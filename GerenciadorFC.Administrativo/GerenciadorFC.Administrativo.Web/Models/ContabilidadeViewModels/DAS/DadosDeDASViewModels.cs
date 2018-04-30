using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.DAS
{
    public class DadosDeDASViewModels
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public string CNPJ { get; set; }
		public string CPF { get; set; }
		public string CodigoContribuite { get; set; }
		public string Url { get; set; }
		public string CodigoAntiCaptcha { get; set; }
		public string mesApuracao { get; set; }
		public string anoApuracao { get; set; }
		public string ValorTributado { get; set; }
		public List<AnexoContribuinteViewModels> Anexo { get; set; }
		public bool Excluido { get; set; }
	}
}
