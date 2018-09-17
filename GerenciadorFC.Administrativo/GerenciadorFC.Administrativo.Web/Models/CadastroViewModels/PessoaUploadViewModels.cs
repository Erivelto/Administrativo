using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.CadastroViewModels
{
    public class PessoaUploadViewModels
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public DateTime DataCriacao { get; set; }
		public Guid Arquivo { get; set; }
		public string Tipo { get; set; }
		public bool Excluido { get; set; }
		public string UrlArquivo { get; set; }
		public string ArquivoCompleto { get; set; }
	}
}
