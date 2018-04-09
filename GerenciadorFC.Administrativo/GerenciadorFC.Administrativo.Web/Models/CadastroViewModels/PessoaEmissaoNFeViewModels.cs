using GerenciadorFC.Administrativo.Web.Enumeradores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.CadastroViewModels
{
    public class PessoaEmissaoNFeViewModels
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }

		public string senha { get; set; }
		[Range(1, int.MaxValue, ErrorMessage = "Select a correct license")]
		public Prefeitura Prefeitura { get; set; }
		public List<DadosNotaViewModels> dadosNota { get; set; }
		public bool Excluido { get; set; }
	}
}
