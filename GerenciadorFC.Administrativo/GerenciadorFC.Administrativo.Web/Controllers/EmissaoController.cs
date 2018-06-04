using GerenciadorFC.Administrativo.Web.Models;
using GerenciadorFC.Administrativo.Web.Models.CadastroViewModels;
using GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models.ContabilidadeDados.Faturamento;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
	[Authorize]
	public class EmissaoController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		public EmissaoController(UserManager<ApplicationUser> userManager)
		{
			this._userManager = userManager;
		}
		public IActionResult Index()
        {
            return View();
        }
		public IActionResult DadosNotaFiscal()
		{
			var notaFiscalViewModels = new NotaFiscalViewModels();
			return View(notaFiscalViewModels);
		}
		public async Task<IActionResult> IncluirDadosNfe(NotaFiscalViewModels notaFiscalViewModels)
		{
			if (ModelState.IsValid)
			{
				using (var clientCont = new HttpClient())
				{
					var userId = _userManager.GetUserId(User);
					var contato = new ContatoViewModels();
					using (var clientContato = new HttpClient())
					{
						clientContato.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Contato/UserId/" + userId);
						var repostaContato = await clientContato.GetAsync("");
						var retornoContato = await repostaContato.Content.ReadAsStringAsync();
						contato = JsonConvert.DeserializeObject<ContatoViewModels>(retornoContato);
					}
					notaFiscalViewModels.CodigoPessoa = contato.CodigoPessoa;
					notaFiscalViewModels.CodigoVerificacao = "";
					notaFiscalViewModels.UrlNfe = "";
					clientCont.BaseAddress = new System.Uri("http://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/NotaFiscal/" + notaFiscalViewModels.NumeroNFE.ToString());
					var reposta = await clientCont.GetAsync("");
					notaFiscalViewModels.DataEnvio = DateTime.Now;
					var notaFiscal = Mapper.Map<NotaFiscalViewModels, NotaFiscal>(notaFiscalViewModels);
					if (reposta.StatusCode.ToString() != "OK")
					{
						using (var clientContPost = new HttpClient())
						{
							clientContPost.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/NotaFiscal");
							var repostaPost = await clientContPost.PostAsJsonAsync("", notaFiscal);
							var retornoPost = await repostaPost.Content.ReadAsStringAsync();

							if (repostaPost.StatusCode.ToString() == "OK")
								notaFiscalViewModels.Incluido = true;
							else
								notaFiscalViewModels.NaoIncluido = true;
						}
					}
				}
			}
			else
			{
				notaFiscalViewModels.NaoIncluido = true;
			}			
			return View("DadosNotaFiscal",notaFiscalViewModels);
		}

	}
}