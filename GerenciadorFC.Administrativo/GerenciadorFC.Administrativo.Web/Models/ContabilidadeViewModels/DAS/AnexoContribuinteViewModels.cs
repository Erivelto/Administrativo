using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.DAS
{
    public class AnexoContribuinteViewModels
    {
		public int Codigo { get; set; }
		public int CodigoDadosDeDAS { get; set; }
		public string Anexo { get; set; }
		public bool Excluido { get; set; }
	}
}
