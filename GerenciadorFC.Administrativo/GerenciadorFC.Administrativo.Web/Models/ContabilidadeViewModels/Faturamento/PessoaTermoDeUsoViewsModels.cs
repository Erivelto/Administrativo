﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.ContabilidadeViewModels.Faturamento
{
    public class PessoaTermoDeUsoViewsModels
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public DateTime DataTermo { get; set; }
		public string UserId { get; set; }
	}
}
