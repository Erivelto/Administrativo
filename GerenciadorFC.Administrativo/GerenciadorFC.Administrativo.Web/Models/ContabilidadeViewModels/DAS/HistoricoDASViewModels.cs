using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.DAS
{
    public class HistoricoDASViewModels
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public int CodigoAnexo { get; set; }
		public int DiaGeracao { get; set; }
		public DateTime DataGeracao { get; set; }
		public string Status { get; set; }
		public string Email { get; set; }
		public bool Excluido { get; set; }
	}
}
