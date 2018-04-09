using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Enumeradores
{
    public enum Prefeitura
    {
		[Description("Guarulhos")]
		Guarulhos,
		[Description("Itaquaquecetuba")]
		Itaquaquecetuba,
		[Description("Arujá")]
		Aruja,
		[Description("São Paulo")]
		SaoPaulo
	}
}
