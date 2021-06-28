using DragonSpark.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	sealed class TypesRegistration : IRegistration
	{
		readonly IServiceCollection _services;
		readonly List<Type>         _types;

		public TypesRegistration(IServiceCollection services, List<Type> types)
		{
			_services = services;
			_types    = types;
		}

		public RegistrationResult Singleton()
		{
			var services = _services;
			var length   = _types.Count;
			for (var i = 0; i < length; i++)
			{
				services = services.AddSingleton(_types[i]);
			}

			var result = services.Result();
			return result;
		}

		public RegistrationResult Transient()
		{
			var services = _services;
			var length   = _types.Count;
			for (var i = 0; i < length; i++)
			{
				services = services.AddTransient(_types[i]);
			}

			var result = services.Result();
			return result;
		}

		public RegistrationResult Scoped()
		{
			var services = _services;
			var length   = _types.Count;
			for (var i = 0; i < length; i++)
			{
				services = services.AddScoped(_types[i]);
			}

			var result = services.Result();
			return result;
		}
	}
}