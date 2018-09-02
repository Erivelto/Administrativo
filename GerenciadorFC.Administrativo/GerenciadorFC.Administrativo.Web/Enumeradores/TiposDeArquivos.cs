using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Enumeradores
{
    public enum TiposDeArquivos
    {
		[Description("Nota Fiscal")]
		NotaFiscal,
		[Description("Impostos")]
		Impostos,
		[Description("Contrato Social")]
		ContratoSocial,
		[Description("Prolabore")]
		Prolabore,
		[Description("IRRF")]
		IRRF
	}
}
