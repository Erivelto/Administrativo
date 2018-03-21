using System.ComponentModel;

namespace GerenciadorFC.Administrativo.Web.Enumeradores
{
    public enum VerboHttp
    {
		/// <summary>
		/// Get
		/// </summary>
		[Description("GET")]
		GET,

		/// <summary>
		/// Post
		/// </summary>
		[Description("POST")]
		POST,

		/// <summary>
		/// Put
		/// </summary>
		[Description("PUT")]
		PUT,

		/// <summary>
		/// Delete
		/// </summary>
		[Description("DELETE")]
		DELETE
	}
}
