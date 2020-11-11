using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Testing.Server
{
	sealed class Accessor : Variable<HttpContext?>, IHttpContextAccessor
	{
		public Accessor(IServiceProvider provider) : base(new DefaultHttpContext {RequestServices = provider}) {}

		public HttpContext? HttpContext
		{
			get => Get();
			set => Execute(value);
		}
	}
}