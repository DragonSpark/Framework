using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Construction
{
	sealed class ServiceScopeFactory : IServiceScopeFactory
	{
		readonly IServiceScopeFactory _factory;

		public ServiceScopeFactory(IServiceScopeFactory factory) => _factory = factory;

		public IServiceScope CreateScope() => new Scope(_factory.CreateScope());

		sealed class Scope : IServiceScope
		{
			readonly IServiceScope _scope;

			public Scope(IServiceScope scope) => _scope = scope;

			public IServiceProvider ServiceProvider => new ActivationAwareServiceProvider(_scope.ServiceProvider);

			public void Dispose()
			{
				_scope.Dispose();
			}
		}
	}
}