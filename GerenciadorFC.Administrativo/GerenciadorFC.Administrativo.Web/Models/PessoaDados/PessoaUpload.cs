using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.PessoaDados
{
	public class PessoaUpload
	{	
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public DateTime DataCriacao { get; set; }
		public Guid Arquivo { get; set; }
		public string Tipo { get; set; }
		public bool Excluido { get; set; }
	}
}
