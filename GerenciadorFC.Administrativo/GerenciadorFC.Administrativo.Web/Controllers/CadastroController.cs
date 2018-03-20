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

namespace GerenciadorFC.Administrativo.Web.Controllers
{

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
		public async Task<ActionResult> Edite(PessoaViewModels pessoaVieModels)
		{
			if (pessoaVieModels.Codigo != 0)
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

							pessoaVieModels = Mapper.Map<Endereco, PessoaViewModels>(_endereco);
							pessoaVieModels = Mapper.Map<Pessoa, PessoaViewModels>(_pessoa);
						}
					}
				}
			}
			return View("Edite", pessoaVieModels);
		}
		public ActionResult Novo()
		{
			return View();
		}
		public async Task<ActionResult> Cadastrar(PessoaViewModels pessoaVieModels)
		{
			if (pessoaVieModels != null)
			{
				pessoaVieModels.DataInclusao = DateTime.Now;
				pessoaVieModels.DataAtulizacao = DateTime.Now;
				var _pessoa = Mapper.Map<PessoaViewModels, Pessoa>(pessoaVieModels);
				var _endereco = Mapper.Map<PessoaViewModels, Endereco>(pessoaVieModels);
				using (var client = new HttpClient())
				{
					client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Pessoa");
					var resposta_p = await client.PostAsJsonAsync("", _pessoa);
					string retorno = await resposta_p.Content.ReadAsStringAsync();
					_pessoa = JsonConvert.DeserializeObject<Pessoa>(retorno);

					if (_pessoa.Codigo != 0)
					{
						using (var clientEnd = new HttpClient())
						{
							_endereco.CodigoPessoa = _pessoa.Codigo;
							clientEnd.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Endereco");
							var reposta_e = await clientEnd.PostAsJsonAsync("", _endereco);
							string retorno_e = await reposta_e.Content.ReadAsStringAsync();
							_endereco = JsonConvert.DeserializeObject<Endereco>(retorno_e);

							pessoaVieModels = Mapper.Map<Endereco, PessoaViewModels>(_endereco);
							pessoaVieModels = Mapper.Map<Pessoa, PessoaViewModels>(_pessoa);							
						}
					}					
				}
			}
			return View("Novo", pessoaVieModels);
		}
	}
}