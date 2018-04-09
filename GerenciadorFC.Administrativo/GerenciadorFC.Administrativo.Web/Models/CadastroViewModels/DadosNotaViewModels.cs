using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.CadastroViewModels
{
    public class DadosNotaViewModels
    {
		public int Codigo { get; set; }
		public DateTime DataEnvio { get; set; }
		public decimal ValorTotal { get; set; }
		public int CodigoServico { get; set; }
		public int NumeroNFE { get; set; }
		public DateTime DataEmissao { get; set; }
		public string CodigoVerificacao { get; set; }
		public List<ItensNotasViewModels> itensNotas { get; set; }
		public bool Excluido { get; set; }
	}
}
