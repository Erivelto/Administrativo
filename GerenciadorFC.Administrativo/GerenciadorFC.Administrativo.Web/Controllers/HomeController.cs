using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
	


	public class HomeController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;


		public HomeController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;			
		}
		[Authorize]
		public async Task<IActionResult> Index()
        {
			var user = await _userManager.GetUserAsync(User);		
			if (user.Status == "termo")
			{
				using (var clientContP = new HttpClient())
				{
					clientContP.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/");
					var respostaTermoP = await clientContP.GetAsync("api/Pessoa/" + user.CodigoPessoa.ToString());
					string dadosTermoP = await respostaTermoP.Content.ReadAsStringAsync();

					var pessoa = JsonConvert.DeserializeObject<Pessoa>(dadosTermoP);
					var pessoaVieModels = Mapper.Map<Pessoa, PessoaViewModels>(pessoa);

					return RedirectToAction("Cobranca", "Account", pessoaVieModels);
				}

			}
			if (user.Status == "pagamentoandamento")
			{

			}
			return View();
        }
		public async Task<IActionResult> PosPagIndex(string email, string status)
		{

			var user =  await _userManager.FindByEmailAsync(email);
			user.Status = status;
			await _userManager.UpdateAsync(user);

			return RedirectToAction("Index");
		}

		public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
		public IActionResult Sair()
		{
			_signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}

    }
}
