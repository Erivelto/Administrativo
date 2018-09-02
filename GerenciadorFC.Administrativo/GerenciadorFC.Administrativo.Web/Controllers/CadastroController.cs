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
using System.Text.RegularExpressions;
using GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento;
using GerenciadorFC.Administrativo.Web.Models.ContabilidadeDados.Faturamento;
using GerenciadorFC.Administrativo.Web.Models.ContabilidadeDados.DAS;
using GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.DAS;
using Microsoft.AspNetCore.Identity;
using GerenciadorFC.Administrativo.Web.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
	[Authorize]
	public class CadastroController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		public async Task<ActionResult> Lista()
		{
			var listaViewsModels = new List<ListaPessoaViewModels>();
			var listaViewsModelsStatus = new List<ListaPessoaViewModels>();
			using (var client = new HttpClient())
			{
				client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Pessoa");
				var resposta = await client.GetAsync("");
				string dados = await resposta.Content.ReadAsStringAsync();

				listaViewsModels = JsonConvert.DeserializeObject<List<ListaPessoaViewModels>>(dados);
			}
			using (var clientStatus = new HttpClient())
			{				
				clientStatus.BaseAddress = new System.Uri("https://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Pessoa/Status");
				var respostaStatus = await clientStatus.GetAsync("");
				string dadosStatus = await respostaStatus.Content.ReadAsStringAsync();

				listaViewsModelsStatus = JsonConvert.DeserializeObject<List<ListaPessoaViewModels>>(dadosStatus);
			}
			return View(getPessoa(listaViewsModels, listaViewsModelsStatus));
		}
		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile file)
		{
			var data = new MemoryStream();
			file.CopyTo(data);
			byte[] imageBytes = data.ToArray();
			var aquivo = Convert.ToBase64String(imageBytes);

			return RedirectToAction("Lista");
		}
		private List<ListaPessoaViewModels> getPessoa(List<ListaPessoaViewModels> listGeral, List<ListaPessoaViewModels> listStatus)
		{
			var result = new List<ListaPessoaViewModels>();
			foreach (var item in listGeral)
			{
				if (listStatus.Where(m => m.Codigo == item.Codigo).FirstOrDefault() != null)
				{
					result.Add(listStatus.Where(m => m.Codigo == item.Codigo).FirstOrDefault());
				}
				else
				{
					result.Add(item);
				}
			}
			return result;
		}
		public async Task<ActionResult> Alterar(PessoaViewModels pessoaVieModels)
		{
			pessoaVieModels.Documento = Regex.Replace(pessoaVieModels.Documento, @"[^\d]", "");
			pessoaVieModels.IncricaoMunicipal = Regex.Replace(pessoaVieModels.IncricaoMunicipal, @"[^\d]", "");
			pessoaVieModels.CEP = Regex.Replace(pessoaVieModels.CEP, @"[^\d]", "");
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

						if (resposta_p.StatusCode.ToString() == "OK")
							pessoaVieModels.Incluido = true;
						else
							pessoaVieModels.NaoIncluido = true;

						if (_pessoa.Codigo != 0)
						{
							using (var clientEnd = new HttpClient())
							{
								_endereco.CodigoPessoa = _pessoa.Codigo;
								clientEnd.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Endereco");
								var reposta_e = await clientEnd.PutAsJsonAsync("", _endereco);
								string retorno_e = await reposta_e.Content.ReadAsStringAsync();
								_endereco = JsonConvert.DeserializeObject<Endereco>(retorno_e);

								if (reposta_e.StatusCode.ToString() == "OK")
									pessoaVieModels.Incluido = true;
								else
									pessoaVieModels.NaoIncluido = true;
							}
						}
					}
				}
				return RedirectToAction("Lista");
			}
			else
			{
				pessoaVieModels.NaoIncluido = true;
				return View("Edite",pessoaVieModels);
			}			
		}
		public async Task<ActionResult> Edite(int pessoa)
		{
			var pessoaVieModels = new PessoaViewModels();
			var listPessoaEmassao = new List<ListaDadosEmissaoNotaViewModels>();
			ViewData["pessoa"] = pessoa;
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
					if(_endereco != null)
						pessoaVieModels = Mapper.Map<Endereco, PessoaViewModels>(_endereco,pessoaVieModels);

					if (pessoaVieModels.CodigoPessoa != 0)
					{
						using (var clientEmissao = new HttpClient())
						{
							clientEmissao.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/DadosEmissaoNota/");
							var respostaEmissao = await clientEmissao.GetAsync("" + pessoaVieModels.CodigoPessoa.ToString());
							string dadoEmissao = await respostaEmissao.Content.ReadAsStringAsync();
							if (dadoEmissao != "")
							{
								var _listPessoaEmassao = JsonConvert.DeserializeObject<ListaDadosEmissaoNotaViewModels>(dadoEmissao);								
								ViewData["CodigoEmissaoNota"] = _listPessoaEmassao.Codigo;
								listPessoaEmassao.Add(_listPessoaEmassao);
								pessoaVieModels.listapessoaEmissaoNFeViewModels = listPessoaEmassao;

								using (var codigo = new HttpClient())
								{
									codigo.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/PessoaCodigoServico/" + pessoa.ToString());
									var respostaCodigo = await codigo.GetAsync("");
									string dadosCodigo = await respostaCodigo.Content.ReadAsStringAsync();
									if (dadosCodigo != "")
									{
										var listCodigo = JsonConvert.DeserializeObject<List<PessoaCodigoServicoViewModels>>(dadosCodigo);
										ViewData["ListaCodigo"] = listCodigo;
									}
								}
								using (var clinteDasRet = new HttpClient())
								{
									clinteDasRet.BaseAddress = new System.Uri("http://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/DadosDeDAS/" + pessoa.ToString());
									var respostaRet = await clinteDasRet.GetAsync("");
									string respostdadosRet = await respostaRet.Content.ReadAsStringAsync();
									if (respostdadosRet != "")
									{
										var DadosCodigo = JsonConvert.DeserializeObject<DadosDeDASViewModels>(respostdadosRet);
										var ListDadosCodigo = new List<DadosDeDASViewModels>();
										ListDadosCodigo.Add(DadosCodigo);
										ViewData["DadosDeDASViewModels"] = ListDadosCodigo;
									}
								}
							}
						}
					}
				}
			}
			var pessoaEmissao = new DadosEmissaoNotaViewModels();
			var dadosDAS = new DadosDeDASViewModels();
			var anexo = new AnexoContribuinteViewModels();
			ViewData["AnexoContribuinteViewModels"] = anexo;
			pessoaVieModels.dadosDeDAS = dadosDAS;
			pessoaVieModels.pessoaEmissaoNFeViewModels = pessoaEmissao;
			return View("Edite",pessoaVieModels);
		}
		public async Task<ActionResult> ExcluirDadosEmissao(int codigo)
		{
			var model = new ListaDadosEmissaoNotaViewModels();
			if (codigo != 0)
			{
				using (var clientCont = new HttpClient())
				{
					clientCont.BaseAddress = new System.Uri("http://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/DadosEmissaoNota?codigo=" + codigo.ToString());
					var reposta = await clientCont.DeleteAsync("");
					var retorno = await reposta.Content.ReadAsStringAsync();
    			}
			}
			return PartialView("_ListDadosFiscaisNovo", model);
		}
		public ActionResult Novo()
		{
			var pessoaViewModels = new PessoaViewModels();
			return View(pessoaViewModels);
		}
		
	    public async Task<ActionResult> PessoaCodigoServico(PessoaCodigoServicoViewModels pessoaCodigoServico)
		{
			if (ModelState.IsValid)
			{
				if (pessoaCodigoServico.CodigoEmissaoNota != 0)
				{
					var _pessoaCodigo = Mapper.Map<PessoaCodigoServicoViewModels, PessoaCodigoServico>(pessoaCodigoServico);
					using (var clientCont = new HttpClient())
					{
						clientCont.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/PessoaCodigoServico");
						var reposta = await clientCont.PostAsJsonAsync("", _pessoaCodigo);
						var retorno = await reposta.Content.ReadAsStringAsync();

						using (var clientContList = new HttpClient())
						{
							clientContList.BaseAddress = new System.Uri("http://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/PessoaCodigoServico/" + pessoaCodigoServico.CodigoEmissaoNota.ToString());
							var respostaCont = await clientContList.GetAsync("");
							string dadosCont = await respostaCont.Content.ReadAsStringAsync();

							var listDados = JsonConvert.DeserializeObject<List<ListaDadosEmissaoNotaViewModels>>(dadosCont);
						}
					}
				}
			}
			RedirectToActionResult redirectResult = new RedirectToActionResult("Edite", "Cadastro", new { @pessoa = (int)ViewData["pessoa"] });
			return redirectResult;
		}
		public ActionResult _DadosAnexo(int codigo)
		{
			var anexo = new AnexoContribuinteViewModels();
			anexo.CodigoDadosDeDAS = codigo;
			return PartialView("_DadosAnexo",anexo);
		}
		public async Task<PartialViewResult> _DadosAnexoLista(int codigo)
		{
			using (var clientContList = new HttpClient())
			{
				clientContList.BaseAddress = new System.Uri("http://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/AnexoContribuinte/ListaPorCodigoDAS/" + codigo.ToString());
				var respostaCont = await clientContList.GetAsync("");
				string dadosCont = await respostaCont.Content.ReadAsStringAsync();

				var listDados = JsonConvert.DeserializeObject<List<AnexoContribuinteViewModels>>(dadosCont);

				return PartialView("_DadosAnexoLista", listDados);
			}			
		}
		public async Task<ActionResult> DadosAnexo(string CodigoDadosDeDAS, string Anexo)
		{
			using (var clienteDas = new HttpClient())
			{
				var anexo = new AnexoContribuinte();
				anexo.CodigoDadosDeDAS = Convert.ToInt16(CodigoDadosDeDAS);
				anexo.Anexo = Anexo;
				anexo.Excluido = false;
				clienteDas.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/AnexoContribuinte");
				var respostaDas = await clienteDas.PostAsJsonAsync("", anexo);
				string dadosDas = await respostaDas.Content.ReadAsStringAsync();
			}
			RedirectToActionResult redirectResult = new RedirectToActionResult("Edite", "Cadastro", new { @pessoa = (int)ViewData["pessoa"] });
			return redirectResult;
		}
		public async Task<ActionResult> DadosDas(string CodigoContribuite, string CPF, string CodigoPessoa)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/");
				var resposta = await client.GetAsync("api/Pessoa/" + CodigoPessoa.ToString());
				string dados = await resposta.Content.ReadAsStringAsync();
				var _pessoa = JsonConvert.DeserializeObject<Pessoa>(dados);

				var dadosDeDAS = new DadosDeDAS();
				dadosDeDAS.CPF = CPF;
				dadosDeDAS.CodigoContribuite = CodigoContribuite;
				dadosDeDAS.CodigoPessoa = Convert.ToInt32(CodigoPessoa);
				dadosDeDAS.CNPJ = _pessoa.Documento;
				dadosDeDAS.mesApuracao = DateTime.Now.Month.ToString();
				dadosDeDAS.anoApuracao = DateTime.Now.Year.ToString();
				dadosDeDAS.ValorTributado = "";

				using (var clienteDas = new HttpClient())
				{
					clienteDas.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/DadosDeDAS");
					var respostaDas = await clienteDas.PostAsJsonAsync("", dadosDeDAS);
					string dadosDas = await respostaDas.Content.ReadAsStringAsync();
				}
			}

			RedirectToActionResult redirectResult = new RedirectToActionResult("Edite", "Cadastro", new { @pessoa = (int)ViewData["pessoa"] });
			return redirectResult;
		}
		public async Task<ActionResult> cadastrarDadosFiscais(DadosEmissaoNotaViewModels dadosEmissaoNotaViewModels)
		{
			if (ModelState.IsValid)
			{
				if (dadosEmissaoNotaViewModels.CodigoPessoa != 0)
				{
					var _dadosEmissao = Mapper.Map<DadosEmissaoNotaViewModels, DadosEmissaoNota>(dadosEmissaoNotaViewModels);
					using (var clientCont = new HttpClient())
					{
						clientCont.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/DadosEmissaoNota");
						var reposta = await clientCont.PostAsJsonAsync("", _dadosEmissao);
						var retorno = await reposta.Content.ReadAsStringAsync();

						using (var clientContList = new HttpClient())
						{
							clientContList.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/DadosEmissaoNota");
							var respostaCont = await clientContList.GetAsync("");
							string dadosCont = await respostaCont.Content.ReadAsStringAsync();

							var listDadoEmissao = JsonConvert.DeserializeObject<List<ListaDadosEmissaoNotaViewModels>>(dadosCont);
							
						}
					}
				}
			}
			RedirectToActionResult redirectResult = new RedirectToActionResult("Edite", "Cadastro", new { @pessoa = dadosEmissaoNotaViewModels.CodigoPessoa.ToString() });
			return redirectResult;
		}
		private async Task<List<ListaDadosEmissaoNotaViewModels>> listaDadosEmissaoNota(int codigoPessoa)
		{
			var listDadoEmissao = new List<ListaDadosEmissaoNotaViewModels>();
			
			return listDadoEmissao;
		}
		public async Task<ActionResult> Ativar(int codigo)
		{
			if (codigo != 0)
			{
				using (var clientCont = new HttpClient())
				{
					HttpContent cont;
					clientCont.BaseAddress = new System.Uri("https://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Contato/Status/" + codigo.ToString());
					var reposta = await clientCont.GetAsync("");
					var retorno = await reposta.Content.ReadAsStringAsync();
				}
			}
			return RedirectToAction("Lista");
		}
		public async Task<ActionResult> Cadastrar(PessoaViewModels pessoaVieModels, int checkboxTC)
		{
			pessoaVieModels.Documento = Regex.Replace(pessoaVieModels.Documento, @"[^\d]", "");
			pessoaVieModels.IncricaoMunicipal = Regex.Replace(pessoaVieModels.IncricaoMunicipal, @"[^\d]", "");
			if (ModelState.IsValid)
			{
				if (pessoaVieModels != null)
				{
					pessoaVieModels.TipoPessoa = checkboxTC;
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
			else
			{
				pessoaVieModels.NaoIncluido = true;
			}
			return View("Novo",pessoaVieModels);
		}
	}
}