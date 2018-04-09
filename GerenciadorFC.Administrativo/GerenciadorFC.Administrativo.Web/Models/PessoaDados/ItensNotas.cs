using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.PessoaDados
{
    public class ItensNotas
    {
		public int Codigo { get; set; }
		public int Quantidade { get; set; }
		public string Descricao { get; set; }
		public decimal ValorUni { get; set; }
		public bool Excluido { get; set; }
	}
}
