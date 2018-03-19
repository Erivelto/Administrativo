using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.PessoaDados
{
    public class PessoaLogin
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public int TipoLogin { get; set; }
		public DateTime DataInclusao { get; set; }
		public DateTime DataUltimoAcesso { get; set; }
		public bool Excluido { get; set; }
	}
}
