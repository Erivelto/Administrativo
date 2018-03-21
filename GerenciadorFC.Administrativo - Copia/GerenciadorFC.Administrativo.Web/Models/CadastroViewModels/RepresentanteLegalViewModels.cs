using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.CadastroViewModels
{
    public class RepresentanteLegalViewModels
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		[DisplayName("Nome")]
		[Required(ErrorMessage = "Nome é obrigatório.")]
		public string Nome { get; set; }
		[DisplayName("CPF")]
		[Required(ErrorMessage = "CPF é obrigatório.")]
		public string CPF { get; set; }
		[DisplayName("RG")]
		[Required(ErrorMessage = "RG é obrigatório.")]
		public string RG { get; set; }
		[DisplayName("Passaporte")]		
		public string Passaporte { get; set; }
		[DisplayName("Data Expedição RG")]
		public DateTime DataExpedicaoRG { get; set; }
		[DisplayName("Data Expedição Passaporte")]
		public DateTime DataExpedicaoPassaporte { get; set; }
		public DateTime DataInclisao { get; set; }
		public DateTime DataAlteracao { get; set; }
		public int Status { get; set; }
		public bool Excluido { get; set; }
	}
}
