using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.PessoaDados
{
    public class PessoaContabil
    {
		public int Codigo { get; set; }
		public string Reference { get; set; }
		public string CodePrepoval { get; set; }
		public string Transacao { get; set; }
		public DateTime DataTransacao { get; set; }
		public DateTime DataPagamento { get; set; }
		public string Status { get; set; }
	}
}
