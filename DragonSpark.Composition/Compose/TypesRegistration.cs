using DragonSpark.Model.Sequences.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	sealed class TypesRegistration : IRegistration
	{
		readonly IServiceCollection _services;
		readonly Lease<Type>        _types;
		readonly uint               _length;

		public TypesRegistration(IServiceCollection services, Lease<Type> types)
			: this(services, types, types.Length) {}

		public TypesRegistration(IServiceCollection services, Lease<Type> types, uint length)
		{
			_services = services;
			_types    = types;
			_length   = length;

		}

		public RegistrationResult Singleton()
		{
			var services = _services;
			for (var i = 0; i < _length; i++)
			{
				services = services.AddSingleton(_types[i]);
			}

			var result = Result(services);
			return result;
		}

		RegistrationResult Result(IServiceCollection services)
		{
			var result = services.Result();
			_types.Dispose();
			return result;
		}

		public RegistrationResult Transient()
		{
			var services = _services;
			for (var i = 0; i < _length; i++)
			{
				services = services.AddTransient(_types[i]);
			}

			var result = services.Result();
			return result;
		}

		public RegistrationResult Scoped()
		{
			var services = _services;
			for (var i = 0; i < _length; i++)
			{
				services = services.AddScoped(_types[i]);
			}

			var result = services.Result();
			return result;
		}
	}
}