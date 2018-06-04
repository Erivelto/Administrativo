using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models;
using GerenciadorFC.Administrativo.Web.Models.AccountViewModels;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using GerenciadorFC.Administrativo.Web.Models.CadastroViewModels;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;
using GerenciadorFC.Administrativo.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Constants.PreApproval;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;

namespace GerenciadorFC.Administrativo.Web.Controllers
{

	[Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null,string tipo = null, string data = null, string email = null)
        {
			var registerViewModel = new RegisterViewModel();
			registerViewModel.CodigoRep = Convert.ToInt32(returnUrl);
			registerViewModel.Email = email == "" ? "" : email;
			registerViewModel.tipo = tipo == "" ? "" : tipo;
			if(data != "")
				registerViewModel.dataExp = Convert.ToDateTime(data);
			
			return View("Register",registerViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email, LockoutEnd = model.dataExp,PhoneNumber = model.tipo == "Contador" ? "admin":"comum" };
				var result = await _userManager.CreateAsync(user, model.Password);
				
				TempData["email"] = model.Email;
				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password.");
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
					await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
					await _signInManager.SignInAsync(user, isPersistent: false);
					_logger.LogInformation("User created a new account with password.");
					if (model.tipo == "Cobrança")
					{
						if (await this.UpdateUserId(Convert.ToInt32(model.CodigoRep), user.Id) == false)
						{
							ContatoViewModels contato = (ContatoViewModels)TempData["listContato"];
							return View("TermoDeUso", contato);
						}
						else
						{
							AddErrors(result);
							TempData["user"] = "comum".ToString();
							return View(model);
						}
					}
					else
					{
						if (model.tipo == "Boleto")
						{
							TempData["Boleto"] = model.tipo;
							if (await this.UpdateUserId(Convert.ToInt32(model.CodigoRep), user.Id) == false)
							{								
								ContatoViewModels contato = (ContatoViewModels)TempData["listContato"];
								return View("TermoDeUso", contato);
							}
							else
							{
								AddErrors(result);
								return View(model);
							}
						}
						else
						{
							TempData["user"] = "admin".ToString();
							AddErrors(result);
							return View(model);
						}
					}
				}
				else
				{
					AddErrors(result);
					return View(model);
				}
			}
			else
			{
				return View(model);
			}

            // If we got this far, something failed, redisplay form
            
        }
		public async Task<IActionResult> TermoDeUso(ContatoViewModels contato)
		{
			return View(contato);
		}
		[AllowAnonymous]
		public async Task<IActionResult> AceiteTermoUso(ContatoViewModels contato)
		{
			var pessoaTermoDeUsoViewModels = new PessoaTermoDeUsoViewModels();
			var pessoaVieModels = new PessoaViewModels();
			pessoaTermoDeUsoViewModels.DataTermo = DateTime.Now;
			pessoaTermoDeUsoViewModels.CodigoPessoa = contato.CodigoPessoa;
			pessoaTermoDeUsoViewModels.UserId = contato.UserId;
			using (var clientCont = new HttpClient())
			{
				clientCont.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/PessoaTermoDeUso");
				var respostaTermo = await clientCont.PostAsJsonAsync("", pessoaTermoDeUsoViewModels);
				string dadosTermo = await respostaTermo.Content.ReadAsStringAsync();

				var termo = JsonConvert.DeserializeObject<PessoaTermoDeUsoViewModels>(dadosTermo);
			}
			using (var clientContP = new HttpClient())
			{
				clientContP.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/");
				var respostaTermoP = await clientContP.GetAsync("api/Pessoa/" + contato.CodigoPessoa.ToString());
				string dadosTermoP = await respostaTermoP.Content.ReadAsStringAsync();

				var pessoa = JsonConvert.DeserializeObject<Pessoa>(dadosTermoP);
				pessoaVieModels = Mapper.Map<Pessoa, PessoaViewModels>(pessoa);
				if (pessoaVieModels != null)
					if (TempData["Boleto"] == null)
					{
						return RedirectToAction("Cobranca", "Account", pessoaVieModels);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}

				else
					return View();
			}
		}
		public async Task<IActionResult> Cobranca(PessoaViewModels pessoaViewModels)
		{
			var pessoaVieModels = (PessoaViewModels)TempData["pessoaVieModels"];
			return View("Cobranca", pessoaViewModels);
		}
		public async Task<IActionResult> CallPagSeguro(string documento, decimal valor, int periodo, string nome)
		{
			bool isSandbox = false;
			EnvironmentConfiguration.ChangeEnvironment(isSandbox);

			// Instantiate a new preApproval request
			PreApprovalRequest preApproval = new PreApprovalRequest();

			// Sets the currency
			preApproval.Currency = Currency.Brl;

			// Sets a reference code for this preApproval request, it is useful to identify this payment in future notifications.
			preApproval.Reference = documento;

			// Sets your customer information.
			preApproval.Sender = new Sender(
				nome,
				TempData["email"].ToString(),
				new Phone("", "")
			);

			// Sets the preApproval informations
			var now = DateTime.Now;
			preApproval.PreApproval = new PreApproval();
			preApproval.PreApproval.Charge = Charge.Auto;
			preApproval.PreApproval.Name = "CONTFY - CONTABILIDADE ONLINE";
			preApproval.PreApproval.AmountPerPayment = valor;
			preApproval.PreApproval.MaxAmountPerPeriod = valor;
			preApproval.PreApproval.MaxPaymentsPerPeriod = 5;
			preApproval.PreApproval.Details = string.Format("Todo dia {0} será cobrado o valor de {1} referente a CONTABILIDADE ONLINE.", now.Day, preApproval.PreApproval.AmountPerPayment.ToString("C2"));
			switch (periodo)
			{
				case 1:
					preApproval.PreApproval.Period = Period.Monthly;
					break;
				case 3:
					preApproval.PreApproval.Period = Period.Trimonthly;
					break;
				case 6:
					preApproval.PreApproval.Period = Period.SemiAnnually;
					break;
				case 12:
					preApproval.PreApproval.Period = Period.Yearly;
					break;
				default:
					break;
			}
			preApproval.PreApproval.DayOfMonth = now.Day;
			preApproval.PreApproval.InitialDate = now;
			preApproval.PreApproval.FinalDate = now.AddMonths(6);
			preApproval.PreApproval.MaxTotalAmount = 1200.00m;

			// Sets the url used by PagSeguro for redirect user after ends checkout process
			preApproval.RedirectUri = new Uri("https://gerenciadorfcadministrativoweb20180319080544.azurewebsites.net/Home/PosPagIndex?transaction_id=E884542-81B3-4419-9A75-BCC6FB495EF1");

			// Sets the url used for user review the signature or read the rules
			//preApproval.ReviewUri = new Uri("http://www.lojamodelo.com.br/revisao");

			SenderDocument senderCPF = new SenderDocument(Documents.GetDocumentByType("CPF"), "27952666878");
			preApproval.Sender.Documents.Add(senderCPF);
			try
			{
				AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);
				Uri preApprovalRedirectUri = preApproval.Register(credentials);
				var pessoaContabil = new PessoaContabil();
				pessoaContabil.DataPagamento = DateTime.Now;
				pessoaContabil.DataTransacao = DateTime.Now;
				pessoaContabil.Transacao = "";
				pessoaContabil.Status = "Novo";
				pessoaContabil.Reference = documento;
				string[] prepoval = preApprovalRedirectUri.ToString().Split("=");
				pessoaContabil.CodePrepoval = prepoval[1].ToString();
				using (var clientCont = new HttpClient())
				{
					clientCont.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/PessoaCobranca");
					var respostaTermo = await clientCont.PostAsJsonAsync("", pessoaContabil);
				}
				TempData["user"] = "comum".ToString();
				return Redirect(preApprovalRedirectUri.ToString());
			}
			catch (PagSeguroServiceException exception)
			{
				Console.WriteLine(exception.Message + "\n");

				foreach (ServiceError element in exception.Errors)
				{
					Console.WriteLine(element + "\n");
				}
				Console.ReadKey();
				return View();
			}
		}
		public async Task<bool> VerificaTermo(int codigoPessoa)
		{
			using (var clientCont = new HttpClient())
			{
				clientCont.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/PessoaTermoDeUso/" + codigoPessoa.ToString());
				var respostaTermo = await clientCont.GetAsync("");
				string dadosTermo = await respostaTermo.Content.ReadAsStringAsync();

				var termo = JsonConvert.DeserializeObject<PessoaTermoDeUsoViewModels>(dadosTermo);

				if (termo == null)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}
		public async Task<bool> UpdateUserId(int idcont, string UserId)
		{
			using (var clientCont = new HttpClient())
			{
				clientCont.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Contato/" + idcont.ToString());
				var respostaCont = await clientCont.GetAsync("");
				string dadosCont = await respostaCont.Content.ReadAsStringAsync();

				var listContato = JsonConvert.DeserializeObject<ContatoViewModels>(dadosCont);
				if (listContato.email != "")
				{
					listContato.UserId = UserId;
					using (var client = new HttpClient())
					{
						client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Contato");
						var resposta = await client.PutAsJsonAsync("", listContato);
						string retorno = await resposta.Content.ReadAsStringAsync();
						
					}
				}

				if (await this.VerificaTermo(listContato.CodigoPessoa) == true)
				{
					return true;
				}
				else
				{
					TempData["listContato"] = listContato;
					return false;
				}
			}
		}
        [HttpPost]        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
