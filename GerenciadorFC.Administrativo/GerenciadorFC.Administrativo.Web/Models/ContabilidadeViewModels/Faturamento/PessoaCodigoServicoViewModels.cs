using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento
{
    public class PessoaCodigoServicoViewModels
    {
		public int Codigo { get; set; }
		public int CodigoEmissaoNota { get; set; }
		public string CodigoServico { get; set; }
		public bool Excluido { get; set; }
	}
}
