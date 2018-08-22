using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.CadastroViewModels
{
    public class ListaPessoaViewModels
    {
		public int Codigo { get; set; }
		public string Nome { get; set; }
		public string Razao { get; set; }
		public string Documento { get; set; }
		public string IncricaoMunicipal { get; set; }
		public DateTime DataInclusao { get; set; }
		public DateTime DataAtulizacao { get; set; }
		public DateTime DataAbertura { get; set; }
		public string DescricaoAtividade { get; set; }
		public string CNAE { get; set; }
		public int Status { get; set; }
		public int TipoPessoa { get; set; }
		public bool Excluido { get; set; }
		public PessoaEmissaoNFeViewModels pessoaEmissaoNFeViewModels { get; set; }
		public List<PessoaEmissaoNFeViewModels> listapessoaEmissaoNFeViewModels { get; set; }
		public string UserStatus { get; set; }
	}
}
