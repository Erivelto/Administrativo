using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
    public class CadastroRepLegalController : Controller
    {
        public IActionResult Novo()
        {
            return View();
        }
    }
}