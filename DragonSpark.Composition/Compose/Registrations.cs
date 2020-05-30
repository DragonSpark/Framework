using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	sealed class Registrations<T> : Registrations
	{
		public Registrations(IServiceCollection services) : base(services, A.Type<T>()) {}
	}

	class Registrations : IRegistrations
	{
		readonly IServiceCollection _services;
		readonly Type               _subject;

		public Registrations(IServiceCollection services, Type subject)
		{
			_services = services;
			_subject  = subject;
		}

		public IRegistration Get(IRelatedTypes parameter) => new TypesRegistration(_services, parameter.Get(_subject));
	}
}