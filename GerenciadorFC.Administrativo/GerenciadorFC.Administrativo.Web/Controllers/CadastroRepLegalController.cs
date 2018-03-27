using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models.CadastroViewModels;
using GerenciadorFC.Administrativo.Web.Models.EnderecoDados;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;
using GerenciadorFC.Administrativo.Web.Models.RepresentanteLegal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
    public class CadastroRepLegalController : Controller
    {
		public async Task<ActionResult> Edite(int rep)
		{
			var repLegalVieModels = new RepresentanteLegalViewModels();
			using (var client = new HttpClient())
			{
				client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/");
				var resposta = await client.GetAsync("api/RepresentanteLegal/" + rep.ToString());
				string dados = await resposta.Content.ReadAsStringAsync();
				var _rep = JsonConvert.DeserializeObject<RepresentanteLegal>(dados);

				using (var clientEnd = new HttpClient())
				{
					clientEnd.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/");
					var respostaEnd = await clientEnd.GetAsync("api/Endereco/-" + rep.ToString());
					string dadosEnd = await respostaEnd.Content.ReadAsStringAsync();

					var _endereco = JsonConvert.DeserializeObject<Endereco>(dadosEnd);

					repLegalVieModels = Mapper.Map<RepresentanteLegal, RepresentanteLegalViewModels>(_rep);
					repLegalVieModels = Mapper.Map<Endereco, RepresentanteLegalViewModels>(_endereco, repLegalVieModels);

				}
			}
			return View("Edite", repLegalVieModels);
		}
		public async Task<ActionResult> Alterar(RepresentanteLegalViewModels representanteLegalViewModels)
		{
			if (ModelState.IsValid)
			{
				if (representanteLegalViewModels.CodigoPessoa != 0)
				{
					representanteLegalViewModels.DataInclisao = DateTime.Now;
					representanteLegalViewModels.DataAlteracao = DateTime.Now;
					var _rep = Mapper.Map<RepresentanteLegalViewModels, RepresentanteLegal>(representanteLegalViewModels);
					var _endereco = Mapper.Map<RepresentanteLegalViewModels, Endereco>(representanteLegalViewModels);
					using (var client = new HttpClient())
					{
						client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/RepresentanteLegal");
						var resposta_p = await client.PutAsJsonAsync("", _rep);
						string retorno = await resposta_p.Content.ReadAsStringAsync();
						_rep = JsonConvert.DeserializeObject<RepresentanteLegal>(retorno);

						if (_rep.Codigo != 0)
						{
							using (var clientEnd = new HttpClient())
							{
								_endereco.CodigoPessoa = _rep.Codigo;
								clientEnd.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Endereco");
								var reposta_e = await clientEnd.PutAsJsonAsync("", _endereco);
								string retorno_e = await reposta_e.Content.ReadAsStringAsync();
								_endereco = JsonConvert.DeserializeObject<Endereco>(retorno_e);

							}
						}
					}
				}
				return RedirectToAction("Lista");
			}
			else
			{
				return View("Edite", representanteLegalViewModels);
			}
		}
		public async Task<ActionResult> Lista()
		{
			var listaViewsModels = new List<ListaRepLegalViewModels>();
			using (var client = new HttpClient())
			{
				client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/RepresentanteLegal");
				var resposta = await client.GetAsync("");
				string dados = await resposta.Content.ReadAsStringAsync();

				listaViewsModels = JsonConvert.DeserializeObject<List<ListaRepLegalViewModels>>(dados);

			}
			return View(listaViewsModels);
		}
		public IActionResult Novo(int pessoa)
        {
			var repLegalViewModel = new RepresentanteLegalViewModels() { CodigoPessoa = pessoa };
			return View(repLegalViewModel);
        }
		public IActionResult _NovoContato(int rep)
		{
			var contatoViewModels = new ContatoViewModels();
			return View(contatoViewModels);
		}
		public async Task<ActionResult> Cadastrar(RepresentanteLegalViewModels repLegalVieModels)
		{
			if (ModelState.IsValid)
			{
				if (repLegalVieModels.CodigoPessoa > 0 )
				{
					repLegalVieModels.DataInclisao = DateTime.Now;
					repLegalVieModels.DataAlteracao = DateTime.Now;
					var _rep = Mapper.Map<RepresentanteLegalViewModels, RepresentanteLegal>(repLegalVieModels);
					var _endereco = Mapper.Map<RepresentanteLegalViewModels, Endereco>(repLegalVieModels);
					using (var client = new HttpClient())
					{
						client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/RepresentanteLegal");
						var resposta_p = await client.PostAsJsonAsync("", _rep);
						string retorno = await resposta_p.Content.ReadAsStringAsync();
						_rep = JsonConvert.DeserializeObject<RepresentanteLegal>(retorno);

						if (_rep.Codigo != 0)
						{
							using (var clientEnd = new HttpClient())
							{
								_endereco.CodigoPessoa = 0;
								_endereco.CodigoRepLegal = _rep.Codigo;
								clientEnd.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Endereco");
								var reposta_e = await clientEnd.PostAsJsonAsync("", _endereco);
								string retorno_e = await reposta_e.Content.ReadAsStringAsync();
								_endereco = JsonConvert.DeserializeObject<Endereco>(retorno_e);
							}
							return RedirectToAction("Lista");
						}
						else
						{
							return View("Novo", repLegalVieModels);
						}
					}
				}
				return RedirectToAction("Lista");
			}
			return View("Novo", repLegalVieModels);
		}
	}
}