using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security;

sealed class HttpContextNonce : ReferenceValueStore<HttpContext, string>
{
	public static HttpContextNonce Default { get; } = new();

	HttpContextNonce() : base(_ => ContentPolicyNonce.Default.Get()) {}
}