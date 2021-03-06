﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeDados.Faturamento
{
    public class CorpoEmissaoNota
    {
		public int Codigo { get; set; }
		public int CodigoEmissaoNota { get; set; }
		public int CodigoPessoa { get; set; }
		public int CodigoTomador { get; set; }
		public string Descricao { get; set; }
		public string Valor { get; set; }
		public DateTime DataPrimeiraEmissao { get; set; }
		public bool repetir { get; set; }
		public string CodigoServico { get; set; }
	}
}
