using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento
{
    public class NotaFiscalViewModels
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public DateTime DataEnvio { get; set; }
		[Display(Name = "Data Emissão")]
		[Required(ErrorMessage = "É obrigatorio data de emissão")]
		public DateTime DataEmissao { get; set; }
		[Display(Name = "Valor da nota fiscal")]
		[Required(ErrorMessage = "É obrigatorio o valor total")]
		public decimal ValorTotal { get; set; }
		[Display(Name = "Número da nota fiscal")]
		[Required(ErrorMessage = "É obrigatorio o numero da nota")]
		public int NumeroNFE { get; set; }
		public string CodigoVerificacao { get; set; }
		public string UrlNfe { get; set; }
		public bool Excluido { get; set; }
	}
}
