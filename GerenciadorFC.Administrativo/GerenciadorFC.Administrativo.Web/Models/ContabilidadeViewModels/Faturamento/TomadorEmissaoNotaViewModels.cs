using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GerenciadorFC.Administrativo.Web.Helps.Validacao;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento
{
	public class TomadorEmissaoNotaViewModels
    {
		public TomadorEmissaoNotaViewModels()
		{
			Incluido = false;
			NaoIncluido = false;
		}
		public int Codigo { get; set; }
		public int CodigoEmissaoNota { get; set; }
		[DisplayName("CNPJ")]
		public string Email { get; set; }
		[DisplayName("CNPJ")]
		[StringLength(15, ErrorMessage = "Digite apenas números")]
		[Cnpj(ErrorMessage = "O valor '{0}' é inválido para CNPJ")]
		[Required(ErrorMessage = "CNPJ é obrigatório.")]
		public string Documento { get; set; }
		[DisplayName("Razão social")]
		[Required(ErrorMessage = "Razão Social é obrigatório.")]
		public string Razao { get; set; }
		[DisplayName("Nome Fantasia")]
		public string Fantasia { get; set; }
		[DisplayName("Endereço")]
		[Required(ErrorMessage = "Endereço é obrigatório.")]
		public string Logradouro { get; set; }
		[DisplayName("Numero")]
		[Required(ErrorMessage = "Numero é obrigatório.")]
		public string NumeroLogradouro { get; set; }
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
		[DisplayName("Inscrição municipal")]
		public string IncrMunicipal { get; set; }
		public string TipoPessoa { get; set; }
		public bool Excluido { get; set; }
		[NotMapped]
		public bool Incluido { get; set; }
		[NotMapped]
		public bool NaoIncluido { get; set; }
	}
}
