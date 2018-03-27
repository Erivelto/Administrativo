using System;
using System.Net.Http;
using System.Threading.Tasks;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models.EnderecoDados;
using GerenciadorFC.Administrativo.Web.Models.CadastroViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
	//[Authorize]
	public class CadastroController : Controller
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
		public async Task<ActionResult> Alterar(PessoaViewModels pessoaVieModels)
		{
			if (ModelState.IsValid)
			{
				if (pessoaVieModels.CodigoPessoa != 0)
				{
					pessoaVieModels.DataInclusao = DateTime.Now;
					pessoaVieModels.DataAtulizacao = DateTime.Now;
					var _pessoa = Mapper.Map<PessoaViewModels, Pessoa>(pessoaVieModels);
					var _endereco = Mapper.Map<PessoaViewModels, Endereco>(pessoaVieModels);
					using (var client = new HttpClient())
					{
						client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Pessoa");
						var resposta_p = await client.PutAsJsonAsync("", _pessoa);
						string retorno = await resposta_p.Content.ReadAsStringAsync();
						_pessoa = JsonConvert.DeserializeObject<Pessoa>(retorno);

						if (_pessoa.Codigo != 0)
						{
							using (var clientEnd = new HttpClient())
							{
								_endereco.CodigoPessoa = _pessoa.Codigo;
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
				return View("Edite",pessoaVieModels);
			}			
		}
		public async Task<ActionResult> Edite(int pessoa)
		{
			var pessoaVieModels = new PessoaViewModels();
			using (var client = new HttpClient())
			{
				client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/");
				var resposta = await client.GetAsync("api/Pessoa/" + pessoa.ToString());
				string dados = await resposta.Content.ReadAsStringAsync();
				var _pessoa = JsonConvert.DeserializeObject<Pessoa>(dados);
				
				using (var clientEnd = new HttpClient())
				{
					clientEnd.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/");
					var respostaEnd = await clientEnd.GetAsync("api/Endereco/" + pessoa.ToString());
					string dadosEnd = await respostaEnd.Content.ReadAsStringAsync();

					var _endereco = JsonConvert.DeserializeObject<Endereco>(dadosEnd);

					pessoaVieModels = Mapper.Map<Pessoa, PessoaViewModels>(_pessoa);
					pessoaVieModels = Mapper.Map<Endereco, PessoaViewModels>(_endereco,pessoaVieModels);

				}
			}
			return View("Edite",pessoaVieModels);
		}
		public ActionResult Novo()
		{
			var pessoaViewModels = new PessoaViewModels();
			return View(pessoaViewModels);
		}
		public async Task<ActionResult> Cadastrar(PessoaViewModels pessoaVieModels)
		{
			if (ModelState.IsValid)
			{
				if (pessoaVieModels != null)
				{
					pessoaVieModels.DataInclusao = DateTime.Now;
					pessoaVieModels.DataAtulizacao = DateTime.Now;
					var _pessoa = Mapper.Map<PessoaViewModels, Pessoa>(pessoaVieModels);
					var _endereco = Mapper.Map<PessoaViewModels, Endereco>(pessoaVieModels);
					using (var client = new HttpClient())
					{
						//client.BaseAddress = new System.Uri("http://localhost:12796/api/Pessoa");
						client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Pessoa");
						var resposta_p = await client.PostAsJsonAsync("", _pessoa);
						string retorno = await resposta_p.Content.ReadAsStringAsync();
						_pessoa = JsonConvert.DeserializeObject<Pessoa>(retorno);

						if (_pessoa.Codigo != 0)
						{
							using (var clientEnd = new HttpClient())
							{
								_endereco.CodigoPessoa = _pessoa.Codigo;
								//clientEnd.BaseAddress = new System.Uri("http://localhost:12796/api/Endereco");
								clientEnd.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Endereco");
								var reposta_e = await clientEnd.PostAsJsonAsync("", _endereco);
								string retorno_e = await reposta_e.Content.ReadAsStringAsync();
								_endereco = JsonConvert.DeserializeObject<Endereco>(retorno_e);
							}
							return RedirectToAction("Lista");
						}
						else
						{
							return View("Novo", pessoaVieModels);
						}
					}
				}
				return RedirectToAction("Lista");
			}
			return View("Novo",pessoaVieModels);
		}
	}
}