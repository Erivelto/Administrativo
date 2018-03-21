using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorFC.Administrativo.Web.Models.Cadastro
{
    public class PessoaViewModels
    {
		public int Codigo { get; set; }
		[DisplayName("Nome Fantasia")]
		[Required(ErrorMessage ="Nome Fantansia é obrigatório.")]
		public string Nome { get; set; }
		[DisplayName("Razão social")]
		[Required(ErrorMessage = "Razão Social é obrigatório.")]
		public string Razao { get; set; }
		[DisplayName("CNPJ")]
		[Required(ErrorMessage = "CNPJ é obrigatório.")]
		public string Documento { get; set; }
		[DisplayName("Inscrição municipal")]
		public string IncricaoMunicipal { get; set; }
		public DateTime DataInclusao { get; set; }
		public DateTime DataAtulizacao { get; set; }
		[DisplayName("Data Abertura")]
		[Required(ErrorMessage = "Data inclusão é obrigatório.")]
		public DateTime DataAbertura { get; set; }
		[DisplayName("Atividade")]
		[Required(ErrorMessage = "Descrição é obrigatório.")]
		public string DescricaoAtividade { get; set; }
		[DisplayName("CNAE")]
		[Required(ErrorMessage = "CNAE é obrigatório.")]
		public string CNAE { get; set; }
		public int Status { get; set; }
		[DisplayName("É contabilidade?")]
		public int TipoPessoa { get; set; }
		public bool Excluido { get; set; }
		public bool Contabilidade { get; set; }


		public int CodigoPessoa { get; set; }
		public int CodigoRepLegal { get; set; }

		public int CodigoEndereco { get; set; }
		[DisplayName("Tipo end.")]
		[Required(ErrorMessage = "Tipo endereço é obrigatório.")]
		public string TipoEnd { get; set; }
		[DisplayName("Endereço")]
		[Required(ErrorMessage = "Endereço é obrigatório.")]
		public string Logradouro { get; set; }
		[DisplayName("Numero")]
		[Required(ErrorMessage = "Numero é obrigatório.")]
		public string Numrero { get; set; }
		[DisplayName("Complemento")]
		public string Complemento { get; set; }
		[DisplayName("Bairro")]
		public string Bairro { get; set; }
		[DisplayName("Cidade")]
		[Required(ErrorMessage = "Cidade é obrigatório.")]
		public string Cidade { get; set; }
		[DisplayName("UF")]
		[Required(ErrorMessage = "UF é obrigatório.")]
		public string UF { get; set; }
		[DisplayName("CEP")]
		[Required(ErrorMessage = "CEP é obrigatório.")]
		public string CEP { get; set; }
		public bool ExcluidoEndereco { get; set; }

	}
}
