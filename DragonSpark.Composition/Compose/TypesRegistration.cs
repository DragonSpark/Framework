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
			var services    = _services;
			var destination = _types.AsSpan();
			for (var i = 0; i < _length; i++)
			{
				services = services.AddSingleton(destination[i]);
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
			var services    = _services;
			var destination = _types.AsSpan();
			for (var i = 0; i < _length; i++)
			{
				services = services.AddTransient(destination[i]);
			}

			var result = Result(services);
			return result;
		}

		public RegistrationResult Scoped()
		{
			var services    = _services;
			var destination = _types.AsSpan();
			for (var i = 0; i < _length; i++)
			{
				services = services.AddScoped(destination[i]);
			}

			var result = Result(services);
			return result;
		}
	}
}