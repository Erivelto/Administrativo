using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento
{
    public class ListaDadosEmissaoNotaViewModels
    {
		public int Codigo { get; set; }		
		public int CodigoPessoa { get; set; }		
		public string Senha { get; set; }		
		public string Prefeitura { get; set; }		
		public string UrlPrefeitura { get; set; }
		public List<PessoaCodigoServicoViewModels> PessoaCodigoServico { get; set; }
		public bool Excluido { get; set; }
	}
}
