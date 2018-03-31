using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Models.CadastroViewModels
{
    public class ContatoViewModels
    {
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public int CodigoRepLegal { get; set; }
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress, ErrorMessage = "E-mail em formato inválido.")]
		public string email { get; set; }
		[Display(Name = "Site")]
		public string Site { get; set; }
		[Display(Name = "DDD")]
		[MaxLength(4,ErrorMessage ="Maximo de caracteres")]
		public string DDD { get; set; }
		[Display(Name = "telefone")]
		[MaxLength(9, ErrorMessage = "Maximo de caracteres")]
		public string Telefone { get; set; }
		[Display(Name = "DDD")]
		[MaxLength(4, ErrorMessage = "Maximo de caracteres")]
		public string DDDC { get; set; }
		[Display(Name = "Celular")]
		[MaxLength(9, ErrorMessage = "Maximo de caracteres")]	
		public string Celular { get; set; }
		public bool Excluido { get; set; }
		public List<ListaContatoViewModels> listaContato { get; set; }

	}
}
