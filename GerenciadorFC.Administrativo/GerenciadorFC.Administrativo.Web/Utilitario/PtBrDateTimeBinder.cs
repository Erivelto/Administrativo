using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFC.Administrativo.Web.Utilitario
{
	public class PtBrDateTimeBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			var value = valueProviderResult.FirstValue;
			DateTime outDate;
			var parsed = DateTime.TryParse(value, CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat,
				DateTimeStyles.None, out outDate);

			var result = ModelBindingResult.Success(outDate);
			if (!parsed)
			{
				result = ModelBindingResult.Failed();
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Data inválida");
			}

			bindingContext.Result = result;

			return Task.FromResult(0);
		}
	}
}
