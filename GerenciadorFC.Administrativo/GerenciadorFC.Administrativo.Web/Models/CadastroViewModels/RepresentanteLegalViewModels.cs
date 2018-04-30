using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorFC.Administrativo.Web.Helps.Validacao;

namespace GerenciadorFC.Administrativo.Web.Models.CadastroViewModels
{
    public class RepresentanteLegalViewModels
    {
		public int CodigoRepLegal { get; set; }
		public int CodigoPessoa { get; set; }
		[DisplayName("Nome")]
		[Required(ErrorMessage = "Nome é obrigatório.")]
		public string Nome { get; set; }
		[StringLength(12, ErrorMessage = "Digite apenas números")]
		[DisplayName("CPF")]
		[Cpf(ErrorMessage = "O valor '{0}' é inválido para CPF")]
		[Required(ErrorMessage = "CPF é obrigatório.")]
		public string CPF { get; set; }
		[DisplayName("RG")]
		[Required(ErrorMessage = "RG é obrigatório.")]
		public string RG { get; set; }
		[StringLength(20, ErrorMessage = "Excedeu o numero de caracteres")]
		[DisplayName("Passaporte")]		
		public string Passaporte { get; set; }
		[DisplayName("Data Expedição RG")]
		[Required(ErrorMessage = "Data  é obrigatório.")]
		[DataType(DataType.Date, ErrorMessage = "Data inválida")]
		public DateTime DataExpedicaoRG { get; set; }
		[DisplayName("Data Expedição Passaporte")]
		[Required(ErrorMessage = "Data  é obrigatório.")]
		[DataType(DataType.Date, ErrorMessage = "Data inválida")]
		public DateTime DataExpedicaoPassaporte { get; set; }
		public DateTime DataInclisao { get; set; }
		public DateTime DataAlteracao { get; set; }
		public int Status { get; set; }
		public bool Excluido { get; set; }
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
		public ContatoViewModels contatoViewModels { get; set; }
		public List<ListaContatoViewModels> listaContato { get; set; }
	}
}
