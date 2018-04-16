using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.CadastroViewModels
{
    public class ListaPessoaEmissaoNFeViewModels
	{
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public string senha { get; set; }
		public string prefeitura { get; set; }
		public string urlPrefeitura { get; set; }
	}
}
