using GerenciadorFC.Administrativo.Web.Enumeradores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace GerenciadorFC.Administrativo.Web.Models.CadastroViewModels
{
	public class PessoaEmissaoNFeViewModels
    {
		public PessoaEmissaoNFeViewModels()
		{
			this.ListPrefeitura = Enum.GetValues(typeof(Prefeitura)).Cast<Prefeitura>().Select(v => new SelectListItem
								{
									Text = v.ToString(),
									Value = ((int)v).ToString()
								}).ToList();
		}
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		[DisplayName("Senha")]
		public string senha { get; set; }
		[DisplayName("Link de acesso")]
		public string urlPrefeitura { get; set; }
		[Range(1, int.MaxValue, ErrorMessage = "Select a correct license")]
		public IEnumerable<SelectListItem> ListPrefeitura { get; set; }
		[DisplayName("Prefeitura")]
		public string prefeitura { get; set; }
		public List<DadosNotaViewModels> dadosNota { get; set; }
		public bool Excluido { get; set; }
	}
}
