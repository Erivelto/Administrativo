using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GerenciadorFC.Administrativo.Web.Models.CadastroViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
    [Route("api/DadosFiscais")]
    public class DadosFiscaisController : Controller
    {
		public async Task<ActionResult> Lista()
		{
			var listaViewsModels = new List<ListaPessoaViewModels>();
			using (var client = new HttpClient())
			{
				client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Pessoa");
				var resposta = await client.GetAsync("");
				string dados = await resposta.Content.ReadAsStringAsync();

				listaViewsModels = JsonConvert.DeserializeObject<List<ListaPessoaViewModels>>(dados);

			}
			return View(listaViewsModels);
		}
	}
}