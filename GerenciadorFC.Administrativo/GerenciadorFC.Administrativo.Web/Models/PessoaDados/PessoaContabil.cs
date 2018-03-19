using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.PessoaDados
{
    public class PessoaContabil
    {
		public int Codigo { get; set; }
		public int CodigoContabil { get; set; }
		public int CodigoPessoa { get; set; }
		public bool Excluido { get; set; }
	}
}
