using GerenciadorFC.Administrativo.Web.Enumeradores;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Net;

namespace GerenciadorFC.Administrativo.Web.Utilitario
{
    public static class ClienteRest
    {
		public static JObject ExecutarMetodoRest(string enderecoDoServico,
												 string templateDeRota,
												 VerboHttp acao,
												 Dictionary<string, string> segmentoDeRota,
												 JObject conteudo,
												 out HttpStatusCode? situacao,
												 out string descricaoDoErro)
		{
			return ExecutarMetodoRest(enderecoDoServico,
											 templateDeRota,
											 acao,
											 segmentoDeRota,
											 null,
											 conteudo,
											 out situacao,
											 out descricaoDoErro);

		}

		public static JObject ExecutarMetodoRest(string enderecoDoServico,
											 string templateDeRota,
											 VerboHttp acao,
											 Dictionary<string, string> segmentoDeRota,
											 Dictionary<string, string> parametro,
											 JObject conteudo,
											 out HttpStatusCode? situacao,
											 out string descricaoDoErro)
		{

			using (var clienteRest = CriarClienteRest(enderecoDoServico))
			{
				var requisicao = new RestRequest(templateDeRota, ExtensaoDeEnumerador.ObterEnumeradorPorNome<Method>(acao.ObterDescricao())) { RequestFormat = DataFormat.Json };

				if (segmentoDeRota != null)
				{
					foreach (var item in (Dictionary<string, string>)segmentoDeRota)
					{
						requisicao.AddUrlSegment(item.Key, item.Value);
					}
				}

				if (parametro != null)
				{
					foreach (var item in (Dictionary<string, string>)parametro)
					{
						requisicao.AddQueryParameter(item.Key, item.Value);
					}
				}

				if (conteudo != null)
				{
					requisicao.JsonSerializer = new JsonSerializer();
					requisicao.AddBody(conteudo);
				}

				var retorno = clienteRest.Execute(requisicao);

				situacao = retorno.StatusCode;
				descricaoDoErro = string.Empty;
				if (situacao != HttpStatusCode.OK && retorno.Content != string.Empty)
					descricaoDoErro = retorno.Content;

				return situacao == HttpStatusCode.OK && !string.IsNullOrEmpty(retorno.Content) && !retorno.Content.Equals("null") ? JObject.Parse(retorno.Content) : null;
			}
		}

		private static ClienteRestDispose CriarClienteRest(string endereco)
		{
			var cliente = new ClienteRestDispose(endereco);
			return cliente;
		}
	}

	public class ClienteRestDispose : RestClient, IDisposable
	{
		public ClienteRestDispose(string endereco)
			: base(endereco) { }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing) { }
		}
	}
}
