using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.EnderecoDados
{
    public class Endereco
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public int CodigoRepLegal { get; set; }
		public string TipoEnd { get; set; }
		public string Logradouro { get; set; }
		public string Numrero { get; set; }
		public string Complemento { get; set; }
		public string Bairro { get; set; }
		public string Cidade { get; set; }
		public string UF { get; set; }
		public string CEP { get; set; }
		public bool Excluido { get; set; }
	}
}
