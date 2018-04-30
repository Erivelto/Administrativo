using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento
{
	public class DadosEmissaoNotaViewModels
	{
		public int Codigo { get; set; }
		[Required(ErrorMessage = "É obrigatorio")]
		public int CodigoPessoa { get; set; }
		[Required(ErrorMessage = "É obrigatorio senha")]
		public string Senha { get; set; }
		[Required(ErrorMessage = "É obrigatorio")]
		public string Prefeitura { get; set; }
		[Required(ErrorMessage = "É obrigatorio")]
		public string UrlPrefeitura { get; set; }
		[NotMapped]
		public List<PessoaCodigoServicoViewModels> PessoaCodigoServico { get; set; }
		public bool Excluido { get; set; }
	}
}
