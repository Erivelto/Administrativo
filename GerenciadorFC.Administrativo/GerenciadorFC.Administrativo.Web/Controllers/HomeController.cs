using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorFC.Administrativo.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
		public IActionResult Index()
        {
            return View();
        }
		public IActionResult PosPagIndex(string transaction_id)
		{

			return View("Index");
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
