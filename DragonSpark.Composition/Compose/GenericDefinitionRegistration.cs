using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose;

public sealed class GenericDefinitionRegistration<T> : GenericDefinitionRegistration
{
	public GenericDefinitionRegistration(IServiceCollection services)
		: base(services, A.Type<T>().GetGenericTypeDefinition()) {}
}

public class GenericDefinitionRegistration : IncludingRegistration
{
	public GenericDefinitionRegistration(IServiceCollection services, Type definition)
		: base(services,
		       new Registrations(services, definition).Then(new RegistrationContext(services, definition))) {}
}