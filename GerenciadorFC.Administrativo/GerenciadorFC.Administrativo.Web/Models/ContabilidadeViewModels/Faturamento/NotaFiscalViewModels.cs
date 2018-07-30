using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento
{
	public class NotaFiscalViewModels
	{
		public NotaFiscalViewModels()
		{
			Incluido = false;
			NaoIncluido = false;
		}
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public DateTime DataEnvio { get; set; }
		[Display(Name = "Data Emissão")]
		[Required(ErrorMessage = "É obrigatorio data de emissão")]
		public DateTime DataEmissao { get; set; }
		[Display(Name = "Valor da nota fiscal")]
		[Required(ErrorMessage = "É obrigatorio o valor total")]
		//[RegularExpression(@"^[0-9]{1,2}([,.][0-9]{1,2})?$", ErrorMessage = "Apenas valor monetário")]
		public decimal ValorTotal { get; set; }
		[Display(Name = "Número da nota fiscal")]
		[Required(ErrorMessage = "É obrigatorio o numero da nota")]		
		public int NumeroNFE { get; set; }
		public string CodigoVerificacao { get; set; }
		public string UrlNfe { get; set; }
		public bool Excluido { get; set; }
		[NotMapped]
		public bool Incluido { get; set; }
		[NotMapped]
		public bool NaoIncluido { get; set; }

		public virtual List<TomadorEmissaoNotaViewModels> TomadorEmissaoNotaViewModelsList { get; set; }

		public virtual CorpoEmissaoNotaViewsModels CorpoEmissaoNotaViewsModels { get; set; }
	}
}
