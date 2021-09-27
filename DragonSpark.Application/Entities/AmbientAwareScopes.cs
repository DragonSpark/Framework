using DragonSpark.Composition;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities
{
	sealed class AmbientAwareScopes : IScopes
	{
		readonly IScopes               _previous;
		readonly IResult<IServiceProvider?> _provider;

		public AmbientAwareScopes(IScopes previous) : this(previous, AmbientProvider.Default) {}

		public AmbientAwareScopes(IScopes previous, IResult<IServiceProvider?> provider)
		{
			_previous = previous;
			_provider = provider;
		}

		public Scope Get()
		{
			var provider = _provider.Get();
			var result = provider != null
				             ? new Scope(provider.GetRequiredService<DbContext>(), EmptyBoundary.Default)
				             : _previous.Get();
			return result;
		}
	}
}