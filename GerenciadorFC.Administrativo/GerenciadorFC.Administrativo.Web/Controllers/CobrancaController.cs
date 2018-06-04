using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Constants.PreApproval;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
	[Authorize]
	public class CobrancaController : Controller
    {
        public IActionResult Index()
        {
			return View();
        }
		public IActionResult Cobranca(PessoaViewModels pessoaViewModels)
		{
			var pessoaVieModels = (PessoaViewModels)TempData["pessoaVieModels"];
			return View("Cobranca", pessoaViewModels);
		}
		public async Task<IActionResult> CallPagSeguro(string documento,decimal valor, int periodo,string nome)
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
	}
}