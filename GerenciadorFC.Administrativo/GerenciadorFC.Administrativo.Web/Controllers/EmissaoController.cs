using GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
	public class EmissaoController : Controller
    {
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
			using (var clientCont = new HttpClient())
			{
				clientCont.BaseAddress = new System.Uri("http://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/NotaFiscal/" + notaFiscalViewModels.NumeroNFE.ToString());
				var reposta = await clientCont.GetAsync("");
				if (reposta.StatusCode.ToString() != "OK")
				{
					using (var clientContPost = new HttpClient())
					{
						clientContPost.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/NotaFiscal");
						var repostaPost = await clientContPost.PostAsJsonAsync("", notaFiscalViewModels);
						var retornoPost = await repostaPost.Content.ReadAsStringAsync();
					}
				}
			}

			return View("DadosNotaFiscal");
		}

	}
}