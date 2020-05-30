using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	sealed class TypesRegistration : IRegistration
	{
		readonly IServiceCollection _services;
		readonly Array<Type>        _types;

		public TypesRegistration(IServiceCollection services, Array<Type> types)
		{
			_services = services;
			_types    = types;
		}

		public RegistrationResult Singleton()
		{
			var services = _services;
			var length   = _types.Length;
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
			var length   = _types.Length;
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
			var length   = _types.Length;
			for (var i = 0; i < length; i++)
			{
				services = services.AddScoped(_types[i]);
			}

			var result = services.Result();
			return result;
		}
	}
}