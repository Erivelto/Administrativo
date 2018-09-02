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
using System.Collections.Generic;
using GerenciadorFC.Administrativo.Web.Models.Cadastro;

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
		public async Task<IActionResult> AgendamentoNotaFiscal()
		{
			var pessoaCodigo = _userManager.GetUserAsync(User).Result.CodigoPessoa;
			var notafiscal = new NotaFiscalViewModels();
			using (var clientTomador = new HttpClient())
			{
				clientTomador.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/TomadorEmissaoNota/" + pessoaCodigo.ToString());
				var respostaTomador = await clientTomador.GetAsync("");
				var retornoTomador = await respostaTomador.Content.ReadAsStringAsync();
				if (retornoTomador != "[]")
					notafiscal.TomadorEmissaoNotaViewModelsList = JsonConvert.DeserializeObject<List<TomadorEmissaoNotaViewModels>>(retornoTomador);
			}
			return View(notafiscal);
		}
		public async Task<IActionResult> ListaAgendamentoNotaFiscal()
		{
			var pessoaCodigo = _userManager.GetUserAsync(User).Result.CodigoPessoa;
			var notafiscal = new List<CorpoEmissaoNota>();
			using (var clientTomador = new HttpClient())
			{
				clientTomador.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/CorpoEmissaoNota/Agendamento/" + pessoaCodigo.ToString());
				var respostaTomador = await clientTomador.GetAsync("");
				var retornoTomador = await respostaTomador.Content.ReadAsStringAsync();
				if (retornoTomador != "[]")
				{
					notafiscal = JsonConvert.DeserializeObject<List<CorpoEmissaoNota>>(retornoTomador);
					return View("ListaAgendamentoNotaFiscal", notafiscal);
				}
				else
				{
					return RedirectToAction("AgendamentoNotaFiscal");
				}
				
			}			
		}
		public async Task<IActionResult> ExcluirAgendamentoNotaFiscal(int codigo)
		{			
			using (var clientTomador = new HttpClient())
			{
				clientTomador.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/CorpoEmissaoNota/" + codigo.ToString());
				var respostaTomador = await clientTomador.DeleteAsync("");
			}
			return RedirectToAction("ListaAgendamentoNotaFiscal");
		}
		public async Task<IActionResult> IncluirTomador()
		{
			var pessoaCodigo = _userManager.GetUserAsync(User).Result.CodigoPessoa;
			var tomadorEmissaoNotaViewModels = new TomadorEmissaoNotaViewModels();
			using (var clientTomador = new HttpClient())
			{
				clientTomador.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/TomadorEmissaoNota/" + pessoaCodigo);
				var respostaTomador = await clientTomador.GetAsync("");
				var retornoTomador = await respostaTomador.Content.ReadAsStringAsync();
				if (retornoTomador != "[]")
					tomadorEmissaoNotaViewModels.TomadorEmissaoNotaViewModelsList = JsonConvert.DeserializeObject<List<TomadorEmissaoNotaViewModels>>(retornoTomador);
			}
			return View(tomadorEmissaoNotaViewModels);
		}
		public async Task<IActionResult> IncluirAgendamento(string tomador,string descricao,string valor,string DataEmissao, string repetir)
		{
			var corpoEmissaoNota = new CorpoEmissaoNota();
			corpoEmissaoNota.CodigoEmissaoNota = 0;
			corpoEmissaoNota.CodigoPessoa = _userManager.GetUserAsync(User).Result.CodigoPessoa;
			corpoEmissaoNota.CodigoServico = "";
			corpoEmissaoNota.CodigoTomador = Convert.ToInt32(tomador);
			corpoEmissaoNota.DataPrimeiraEmissao = Convert.ToDateTime(DataEmissao);
			corpoEmissaoNota.Descricao = descricao;
			corpoEmissaoNota.Valor = valor;
			if (repetir == "1")
				corpoEmissaoNota.repetir = true;
			using (var client = new HttpClient())
			{
				client.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/CorpoEmissaoNota");
				var resposta = await client.PostAsJsonAsync("", corpoEmissaoNota);
				string retorno = await resposta.Content.ReadAsStringAsync();
			}
			return RedirectToAction("ListaAgendamentoNotaFiscal");
		}
		public async Task<IActionResult> ExcluirTomador(Int32 codigo)
		{
			if (codigo != 0)
			{
				using (var clientCont = new HttpClient())
				{
					clientCont.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/TomadorEmissaoNota?codigo=" + codigo.ToString());
					var reposta = await clientCont.DeleteAsync("");
					var retorno = await reposta.Content.ReadAsStringAsync();
				}
			}			
			return RedirectToAction("IncluirTomador");
		}
		public async Task<IActionResult> SalvarTomador(TomadorEmissaoNotaViewModels tomadorEmissaoNotaViewModels)
		{
			var userId = _userManager.GetUserAsync(User).Result.CodigoPessoa;
			tomadorEmissaoNotaViewModels.CodigoEmissaoNota =Convert.ToInt32(userId);
			var tomadorEmissaoNota = Mapper.Map<TomadorEmissaoNotaViewModels, TomadorEmissaoNota>(tomadorEmissaoNotaViewModels);
			using (var client = new HttpClient())
			{
				client.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/TomadorEmissaoNota");
				var resposta_p = await client.PostAsJsonAsync("", tomadorEmissaoNota);
				string retorno = await resposta_p.Content.ReadAsStringAsync();
				if (resposta_p.StatusCode.ToString() == "OK")
				{
					tomadorEmissaoNotaViewModels.Incluido = true;
				}
				else
				{
					tomadorEmissaoNotaViewModels.NaoIncluido = true;
				}
				using (var clientTomador = new HttpClient())
				{
					clientTomador.BaseAddress = new System.Uri("https://gerenciadorfccontabilidadeservico20180428013121.azurewebsites.net/api/TomadorEmissaoNota/" + userId);
					var respostaTomador = await clientTomador.GetAsync("");
					var retornoTomador = await respostaTomador.Content.ReadAsStringAsync();
					if (retornoTomador != "[]")
						tomadorEmissaoNotaViewModels.TomadorEmissaoNotaViewModelsList = JsonConvert.DeserializeObject<List<TomadorEmissaoNotaViewModels>>(retornoTomador);
				}

			}
			return View("IncluirTomador", tomadorEmissaoNotaViewModels);
		}
		public async Task<IActionResult> IncluirDadosNfe(NotaFiscalViewModels notaFiscalViewModels)
		{
			if (ModelState.IsValid)
			{
				using (var clientCont = new HttpClient())	
				{
					var pessoaCodigo = _userManager.GetUserAsync(User).Result.CodigoPessoa;
					if (pessoaCodigo != 0)
					{
						notaFiscalViewModels.CodigoPessoa = pessoaCodigo;
						notaFiscalViewModels.CodigoVerificacao = "";
						notaFiscalViewModels.UrlNfe = "";
						if (notaFiscalViewModels.NumeroNFE > 0)
						{
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
							else
							{
								notaFiscalViewModels.Incluido = true;
							}
						}
						else
						{
							var notaFiscal = Mapper.Map<NotaFiscalViewModels, NotaFiscal>(notaFiscalViewModels);
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
					else
					{
						notaFiscalViewModels.NaoIncluido = true;
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