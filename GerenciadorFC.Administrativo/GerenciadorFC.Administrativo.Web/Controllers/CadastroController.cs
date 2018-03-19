using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models.EnderecoDados;

namespace GerenciadorFC.Administrativo.Web.Controllers
{

	public class CadastroController : Controller
    {
		public ActionResult Novo()
		{
			return View();
		}
		public async Task<ActionResult> Cadastrar(PessoaViewModels pessoaVieModels)
		{
			if (pessoaVieModels != null)
			{
				var _pessoa = Mapper.Map<PessoaViewModels, Pessoa>(pessoaVieModels);
				var _endereco = Mapper.Map<PessoaViewModels, Endereco>(pessoaVieModels);
				pessoaVieModels.DataInclusao = DateTime.Now;
				pessoaVieModels.DataAtulizacao = DateTime.Now;
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