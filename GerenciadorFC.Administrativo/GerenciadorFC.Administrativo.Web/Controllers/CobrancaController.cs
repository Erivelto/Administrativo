using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
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

	}
}