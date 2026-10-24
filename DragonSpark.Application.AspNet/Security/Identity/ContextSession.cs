using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class ContextSession<T> : ReferenceValueStore<HttpContext, SignInManager<T>>, IContextSession<T> where T : class
{
	public ContextSession(ComposeContextSession<T> compose) : base(compose) {}
}