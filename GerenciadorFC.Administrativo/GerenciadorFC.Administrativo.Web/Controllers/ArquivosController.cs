using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models;
using GerenciadorFC.Administrativo.Web.Models.CadastroViewModels;
using GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento;
using GerenciadorFC.Administrativo.Web.Models.PessoaDados;
using GerenciadorFC.Administrativo.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
    public class ArquivosController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly ILogger _logger;

		public ArquivosController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender,
			ILogger<AccountController> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_logger = logger;
		}

		public async Task<IActionResult> ListaNotaFiscal()
        {
			var pessoaUploadViewModels = new List<PessoaUploadViewModels>();
			var user =  await _userManager.GetUserAsync(User);
			var codigoPessoa = user.CodigoPessoa;
			
			using (var clientContList = new HttpClient())
			{
				clientContList.BaseAddress = new System.Uri("https://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/PessoaUpload/" + codigoPessoa.ToString() + "/" + "Nota Fiscal");
				var respostaCont = await clientContList.GetAsync("");
				string dadosCont = await respostaCont.Content.ReadAsStringAsync();

				var listDados = JsonConvert.DeserializeObject<List<PessoaUpload>>(dadosCont);
				foreach (var item in listDados)
				{
					var pessoaUpload = Mapper.Map<PessoaUpload, PessoaUploadViewModels>(item);

					using (var client = new HttpClient())
					{
						client.BaseAddress = new System.Uri("http://armazemantodearquivocontfy.azurewebsites.net/ArmazenamentoDeObjeto/UrlArquivo/" + item.Arquivo.ToString() + "/"+ item.CodigoPessoa.ToString());
						var resposta = await client.GetAsync("");
						var dados = await resposta.Content.ReadAsAsync<string>();

						pessoaUpload.UrlArquivo = dados;

						pessoaUploadViewModels.Add(pessoaUpload);
					}					
				}
			}
			return View(pessoaUploadViewModels);
        }
		public async Task<IActionResult> ListaContatoSocial()
		{
			var pessoaUploadViewModels = new List<PessoaUploadViewModels>();
			var user = await _userManager.GetUserAsync(User);
			var codigoPessoa = user.CodigoPessoa;

			using (var clientContList = new HttpClient())
			{
				clientContList.BaseAddress = new System.Uri("https://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/PessoaUpload/" + codigoPessoa.ToString() + "/" + "Contrato Social");
				var respostaCont = await clientContList.GetAsync("");
				string dadosCont = await respostaCont.Content.ReadAsStringAsync();

				var listDados = JsonConvert.DeserializeObject<List<PessoaUpload>>(dadosCont);
				foreach (var item in listDados)
				{
					var pessoaUpload = Mapper.Map<PessoaUpload, PessoaUploadViewModels>(item);

					using (var client = new HttpClient())
					{
						client.BaseAddress = new System.Uri("http://armazemantodearquivocontfy.azurewebsites.net/ArmazenamentoDeObjeto/UrlArquivo/" + item.Arquivo.ToString() + "/" + item.CodigoPessoa.ToString());
						var resposta = await client.GetAsync("");
						var dados = await resposta.Content.ReadAsAsync<string>();

						pessoaUpload.UrlArquivo = dados;

						pessoaUploadViewModels.Add(pessoaUpload);
					}
				}
			}
			return View(pessoaUploadViewModels);
		}
		public async Task<IActionResult> ListaImposto()
		{
			var pessoaUploadViewModels = new List<PessoaUploadViewModels>();
			var user = await _userManager.GetUserAsync(User);
			var codigoPessoa = user.CodigoPessoa;

			using (var clientContList = new HttpClient())
			{
				clientContList.BaseAddress = new System.Uri("https://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/PessoaUpload/" + codigoPessoa.ToString() + "/" + "Impostos");
				var respostaCont = await clientContList.GetAsync("");
				string dadosCont = await respostaCont.Content.ReadAsStringAsync();

				var listDados = JsonConvert.DeserializeObject<List<PessoaUpload>>(dadosCont);
				foreach (var item in listDados)
				{
					var pessoaUpload = Mapper.Map<PessoaUpload, PessoaUploadViewModels>(item);

					using (var client = new HttpClient())
					{
						client.BaseAddress = new System.Uri("http://armazemantodearquivocontfy.azurewebsites.net/ArmazenamentoDeObjeto/UrlArquivo/" + item.Arquivo.ToString() + "/" + item.CodigoPessoa.ToString());
						var resposta = await client.GetAsync("");
						var dados = await resposta.Content.ReadAsAsync<string>();

						pessoaUpload.UrlArquivo = dados;

						pessoaUploadViewModels.Add(pessoaUpload);
					}
				}
			}
			return View(pessoaUploadViewModels);
		}
		public async Task<IActionResult> ListaIRRF()
		{
			var pessoaUploadViewModels = new List<PessoaUploadViewModels>();
			var user = await _userManager.GetUserAsync(User);
			var codigoPessoa = user.CodigoPessoa;

			using (var clientContList = new HttpClient())
			{
				clientContList.BaseAddress = new System.Uri("https://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/PessoaUpload/" + codigoPessoa.ToString() + "/" + "Impostos");
				var respostaCont = await clientContList.GetAsync("");
				string dadosCont = await respostaCont.Content.ReadAsStringAsync();

				var listDados = JsonConvert.DeserializeObject<List<PessoaUpload>>(dadosCont);
				foreach (var item in listDados)
				{
					var pessoaUpload = Mapper.Map<PessoaUpload, PessoaUploadViewModels>(item);

					using (var client = new HttpClient())
					{
						client.BaseAddress = new System.Uri("http://armazemantodearquivocontfy.azurewebsites.net/ArmazenamentoDeObjeto/UrlArquivo/" + item.Arquivo.ToString() + "/" + item.CodigoPessoa.ToString());
						var resposta = await client.GetAsync("");
						var dados = await resposta.Content.ReadAsAsync<string>();

						pessoaUpload.UrlArquivo = dados;

						pessoaUploadViewModels.Add(pessoaUpload);
					}
				}
			}
			return View(pessoaUploadViewModels);
		}
		public async Task<IActionResult> ListaProlabore()
		{
			var pessoaUploadViewModels = new List<PessoaUploadViewModels>();
			var user = await _userManager.GetUserAsync(User);
			var codigoPessoa = user.CodigoPessoa;

			using (var clientContList = new HttpClient())
			{
				clientContList.BaseAddress = new System.Uri("https://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/PessoaUpload/" + codigoPessoa.ToString() + "/" + "Prolabore");
				var respostaCont = await clientContList.GetAsync("");
				string dadosCont = await respostaCont.Content.ReadAsStringAsync();

				var listDados = JsonConvert.DeserializeObject<List<PessoaUpload>>(dadosCont);
				foreach (var item in listDados)
				{
					var pessoaUpload = Mapper.Map<PessoaUpload, PessoaUploadViewModels>(item);

					using (var client = new HttpClient())
					{
						client.BaseAddress = new System.Uri("http://armazemantodearquivocontfy.azurewebsites.net/ArmazenamentoDeObjeto/UrlArquivo/" + item.Arquivo.ToString() + "/" + item.CodigoPessoa.ToString());
						var resposta = await client.GetAsync("");
						var dados = await resposta.Content.ReadAsAsync<string>();

						pessoaUpload.UrlArquivo = dados;

						pessoaUploadViewModels.Add(pessoaUpload);
					}
				}
			}
			return View(pessoaUploadViewModels);
		}
	}
}