using AutoMapper;
using GerenciadorFC.Administrativo.Web.Models;
using GerenciadorFC.Administrativo.Web.Models.CadastroViewModels;
using GerenciadorFC.Administrativo.Web.Models.EnderecoDados;
using GerenciadorFC.Administrativo.Web.Models.RepresentanteLegal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Controllers
{
	public class CadastroRepLegalController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;

		public CadastroRepLegalController(UserManager<ApplicationUser> userManager)
		{
			this._userManager = userManager;
		}

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
					_endereco.CodigoPessoa = repLegalVieModels.CodigoPessoa;
					repLegalVieModels = Mapper.Map<Endereco, RepresentanteLegalViewModels>(_endereco, repLegalVieModels);

				}
				using (var clientCont = new HttpClient())
				{
					clientCont.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Contato?" + "codigoRep=" +_rep+ "&val=0");
					var respostaCont = await clientCont.GetAsync("");
					string dadosCont = await respostaCont.Content.ReadAsStringAsync();

					var listContato = JsonConvert.DeserializeObject<List<ListaContatoViewModels>>(dadosCont);
					if(listContato.Count > 0 ) 
						repLegalVieModels.listaContato  = listContato;
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
		public async Task<ActionResult> ExcluirContato(int contato)
		{
			var model = new ContatoViewModels();
			if (contato != 0)
			{
				using (var clientCont = new HttpClient())
				{
					clientCont.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Contato?codigo=" + contato.ToString());
					var reposta = await clientCont.DeleteAsync("");
					var retorno = await reposta.Content.ReadAsStringAsync();

					using (var clientContList = new HttpClient())
					{
						clientContList.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Contato");
						var respostaCont = await clientContList.GetAsync("");
						string dadosCont = await respostaCont.Content.ReadAsStringAsync();

						var listContato = JsonConvert.DeserializeObject<List<ListaContatoViewModels>>(dadosCont);
						model.listaContato = listContato;
					}
				}
			}
			return PartialView("_ListaContato", model.listaContato);
		}
		public async Task<ActionResult> EnviarEmailContato(string email, int codigo,string tipo, string data)
		{			
			var url = "https://localhost:44340/Account/Register?returnUrl=" + codigo.ToString() + "&tipo=" + tipo + "&data=" + data + "&email=" + email;
			var corpoEmail = "<html xmlns='http://www.w3.org/1999/xhtml'><head>	<style type='text/css'>		body, #bodyTable, #bodyCell, #bodyCell {			height: 100% !important;			margin: 0;			padding: 0;			width: 100% !important;			font-family: Helvetica, Arial, 'Lucida Grande', sans-serif;		}		body, table, td, p, a, li, blockquote {			-ms-text-size-adjust: 100%;			-webkit-text-size-adjust: 100%;			font-weight: normal !important;		}		body, #bodyTable {			background-color: #E1E1E1;		}	</style>	<script type='text/javascript' src='http://gc.kis.v2.scr.kaspersky-labs.com/D23BDC46-5991-6E46-B0E9-985879E9A50E/main.js' charset='UTF-8'></script></head><body bgcolor='#E1E1E1' leftmargin='0' marginwidth='0' topmargin='0' marginheight='0' offset='0'><center style='background-color:#E1E1E1;'><table border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable' style='table-layout: fixed;max-width:100% !important;width: 100% !important;min-width: 100% !important;'><tr><td align='center' valign='top' id='bodyCell'><table bgcolor='#FFFFFF' border='0' cellpadding='0' cellspacing='0' width='500' id='emailBody'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%' style='color:#FFFFFF;' bgcolor='#F8F8F8'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'><table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td align='left' valign='top' class='textContent' style='color: dodgerblue; '><h1>Contfy</h1>  </td></tr></table></td></tr></table></td></tr></table></td></tr><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#FFFFFF'><tr>	<td align='center' valign='top'>		<table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'><table border='0' cellpadding='30' cellspacing='0' width='100%'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td valign='top' class='textContent'><h3 style='color:#5F5F5F;line-height:125%;font-family:Helvetica,Arial,sans-serif;font-size:20px;font-weight:normal;margin-top:0;margin-bottom:3px;text-align:left;'>Bem vindo ao primeiro acesso ao sistema da CONTFY:</h3><br /> </td></tr></table></td>			</tr></table></td></tr></table></td></tr></table></td></tr><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%'>										<tr style='padding-top:0;'>									<td align='center' valign='top'>																								<table border='0' cellpadding='30' cellspacing='0' width='500' class='flexibleContainer'>													<tr>														<td style='padding-top:0;' align='center' valign='top' width='500' class='flexibleContainerCell'>															<table border='0' cellpadding='0' cellspacing='0' width='50%' class='emailButton' style='background-color: #3498DB;'>																<tr>																<td align='center' valign='middle' class='buttonContent' style='padding-top:15px;padding-bottom:15px;padding-right:15px;padding-left:15px;'>																		<a style='color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;' href='" + url + "' target='_blank'>Acesse aqui</a>																	</td>																</tr>														</table>																							</td>													</tr>												</table>																						</td>										</tr>									</table>																	</td>							</tr><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#FFFFFF'><tr><td align='center' valign='top'><table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'><tr><td align='center' valign='top' width='500' class='flexibleContainerCell'>						<table border='0' cellpadding='30' cellspacing='0' width='100%'>															<tr>																<td align='center' valign='top'>																	<table border='0' cellpadding='0' cellspacing='0' width='100%'>																		<tr>																			<td valign='top' class='textContent'>																				<div style='text-align:left;font-family:Helvetica,Arial,sans-serif;font-size:15px;margin-bottom:0;color:#5F5F5F;line-height:135%;'>CONTFY tem a oferecer aos nossos clientes a tecnologia às suas mãos para facilitar e agilizar todo esse processo burocrático na qual, estamos preparado, tanto na contabilidade tradicional quanto na online.</div>																			</td>																		</tr>																	</table>																</td>															</tr>														</table>													</td>												</tr>											</table>										</td>									</tr>								</table>							</td>					</tr>					</table>					<table bgcolor='#E1E1E1' border='0' cellpadding='0' cellspacing='0' width='500' id='emailFooter'>						<tr>							<td align='center' valign='top'>								<table border='0' cellpadding='0' cellspacing='0' width='100%'>									<tr>										<td align='center' valign='top'>											<!-- FLEXIBLE CONTAINER // -->											<table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'>												<tr>													<td align='center' valign='top' width='500' class='flexibleContainerCell'>														<table border='0' cellpadding='30' cellspacing='0' width='100%'>															<tr>																<td valign='top' bgcolor='#E1E1E1'>																	<div style='font-family:Helvetica,Arial,sans-serif;font-size:13px;color:#828282;text-align:center;line-height:120%;'>																		<div>Contfy &#169;  a sua contabilidade online.</div>																	</div>																</td>															</tr>														</table>													</td>												</tr>											</table>										</td>									</tr>								</table>							</td>						</tr>					</table>				</td>			</tr>		</table>	</center></body></html>";
			MailMessage mail = new MailMessage();

			mail.From = new MailAddress("contfy@contfy.com.brm");
			mail.To.Add(email);
			mail.Subject = "Acesso ao sistema!";
			mail.Body = corpoEmail;
			mail.IsBodyHtml = true;
		
			SmtpClient smtp = new SmtpClient();
			smtp.Host = "smtp.gmail.com";
			smtp.EnableSsl = true;
			smtp.Port = 587;
			smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtp.UseDefaultCredentials = false;
			smtp.Credentials = new NetworkCredential("contfy@contfy.com.br", "erivelto33");
			try
			{
				smtp.Send(mail);
			}
			catch (Exception ex)
			{
				throw;
			}			
			return PartialView("_ListaContato");
		}
		public async Task<ActionResult> CadastraContato(ContatoViewModels contatoViewModels)
		{
			var validaemail = await _userManager.FindByNameAsync(contatoViewModels.email);
			if (validaemail == null)
			{
				var model = new ContatoViewModels();
				if (contatoViewModels.CodigoRepLegal != 0)
				{
					using (var clientCont = new HttpClient())
					{
						clientCont.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Contato");
						var reposta = await clientCont.PostAsJsonAsync("", contatoViewModels);
						var retorno = await reposta.Content.ReadAsStringAsync();

						using (var clientContList = new HttpClient())
						{
							clientContList.BaseAddress = new System.Uri("http://gerenciadorfccadastroservicos20180317071207.azurewebsites.net/api/Contato");
							var respostaCont = await clientContList.GetAsync("");
							string dadosCont = await respostaCont.Content.ReadAsStringAsync();

							var listContato = JsonConvert.DeserializeObject<List<ListaContatoViewModels>>(dadosCont);
							model.listaContato = listContato;
						}
					}
				}
				return PartialView("_ListaContato", model.listaContato);
			}
			else
			{
				ModelState.AddModelError("email", "Esse email já está cadastrado!");
				return PartialView("_NovoContato", contatoViewModels);
			}

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