using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Utilitario
{
    public static class ExtensaoDeEnumerador
    {
		/// <summary>
		/// Obtem a descrição do enumerador
		/// </summary>
		/// <param name="valorDoEnumerador">Enumerador selecionado</param>
		/// <returns>Retorna o que está configurado na tag Description</returns>
		public static string ObterDescricao(this Enum valorDoEnumerador)
		{
			var itensDoEnumerador = valorDoEnumerador.ToString().Split(',');
			var decricao = new string[itensDoEnumerador.Length];

			for (var i = 0; i < itensDoEnumerador.Length; i++)
			{
				var informacoesDoCampo = valorDoEnumerador.GetType().GetField(itensDoEnumerador[i]?.Trim());
				var atributos = (DescriptionAttribute[])informacoesDoCampo.GetCustomAttributes(typeof(DescriptionAttribute), false);
				decricao[i] = (atributos.Length > 0) ? atributos[0].Description : itensDoEnumerador[i]?.Trim();
			}

			return string.Join(", ", decricao);
		}

		/// <summary>
		/// Obtem a descrição do enumerador
		/// </summary>
		/// <param name="valorDoEnumerador">Enumerador selecionado</param>
		/// <returns>Retorna o que está configurado na tag Description</returns>
		public static string ObterDescricao(Enum valorDoEnumerador, bool teste)
		{
			var itensDoEnumerador = valorDoEnumerador.ToString().Split(',');
			var decricao = new string[itensDoEnumerador.Length];

			for (var i = 0; i < itensDoEnumerador.Length; i++)
			{
				var informacoesDoCampo = valorDoEnumerador.GetType().GetField(itensDoEnumerador[i]?.Trim());
				var atributos = (DescriptionAttribute[])informacoesDoCampo.GetCustomAttributes(typeof(DescriptionAttribute), false);
				decricao[i] = (atributos.Length > 0) ? atributos[0].Description : itensDoEnumerador[i]?.Trim();
			}

			return string.Join(", ", decricao);
		}



		/// <summary>
		/// Obtem o item do enumerador pela descrição.
		/// </summary>
		/// <typeparam name="T">Tipo do enumerador.</typeparam>
		/// <param name="descricao">Descrição do item.</param>
		/// <returns>Retorna o enumerador selecionado.</returns>
		public static T ObterEnumerador<T>(string descricao)
		{
			if (descricao == null)
				return default(T);

			descricao = descricao.ToUpper();

			var tipo = typeof(T);

			if (!tipo.IsEnum)
			{
				throw new InvalidOperationException();
			}

			foreach (var campo in tipo.GetFields())
			{
				var atributo = Attribute.GetCustomAttribute(campo, typeof(DescriptionAttribute)) as DescriptionAttribute;

				if (atributo != null)
				{
					if (atributo.Description.ToUpper() == descricao)
					{
						return (T)campo.GetValue(null);
					}
				}
				else
				{
					if (campo.Name.ToUpper() == descricao)
					{
						return (T)campo.GetValue(null);
					}
				}
			}

			throw new InvalidCastException(string.Format("Enumerador {0} com a descrição {1} não encontrado", tipo, descricao));
		}

		/// <summary>
		/// Obtem o item do enumerador pelo nome do campo.
		/// </summary>
		/// <typeparam name="T">Tipo do enumerador.</typeparam>
		/// <param name="descricao">Nome do campo.</param>
		/// <returns>Retorna o enumerador selecionado.</returns>
		public static T ObterEnumeradorPorNome<T>(string nome)
		{
			if (string.IsNullOrEmpty(nome))
				return default(T);

			nome = nome.ToUpper();

			var tipo = typeof(T);

			if (!tipo.IsEnum)
			{
				throw new InvalidOperationException();
			}

			foreach (var campo in tipo.GetFields())
			{
				if (campo.Name.ToUpper() == nome)
				{
					return (T)campo.GetValue(null);
				}
			}

			throw new InvalidCastException(string.Format("Enumerador {0} com o nome {1} não encontrado", tipo, nome));
		}

		public static string GetEnumDescription<T>(string value)
		{
			Type type = typeof(T);
			var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

			if (name == null)
			{
				return string.Empty;
			}

			var field = type.GetField(name);
			var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
		}

		/// <summary>
		/// Converte uma classe Enumerador para um IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="tipoCase">null - mesma descricao como no enumerador, U - converte para maiuscula, L - converte para minusculas</param>
		/// <returns></returns>
		public static IEnumerable<SelectListItem> EnumToSelectList<T>(string tipoCase = null)
		{
			return (Enum.GetValues(typeof(T)).Cast<T>().Select(
				e => new SelectListItem()
				{
					Text = (tipoCase == null ? GetEnumDescription<T>(e.ToString()) : (tipoCase.ToUpper() == "U" ? GetEnumDescription<T>(e.ToString()).ToUpper() : GetEnumDescription<T>(e.ToString()).ToLower())),
					Value = e.ToString()
				})).ToList();
		}
	}
}
