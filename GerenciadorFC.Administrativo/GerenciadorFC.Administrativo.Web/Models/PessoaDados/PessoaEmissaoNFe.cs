using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.PessoaDados
{
    public class PessoaEmissaoNFe
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public string senha { get; set; }
		public List<DadosNota> dadosNota { get; set; }
		public bool Excluido { get; set; }
	}
}
