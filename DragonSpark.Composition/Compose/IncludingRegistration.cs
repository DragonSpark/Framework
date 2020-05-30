using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	public class IncludingRegistration : IIncludingRegistration
	{
		readonly IRegistrations _current;

		public IncludingRegistration(IServiceCollection services, IRegistrationContext context)
			: this(services, context.Adapt()) {}

		public IncludingRegistration(IServiceCollection services, IRegistration next)
			: this(services, next.Fixed()) {}

		public IncludingRegistration(IServiceCollection services, IRegistrations current)
		{
			_current = current;
			Services = services;
		}

		protected IServiceCollection Services { get; }

		protected IRegistrations Next(IRegistration next) => Next(next.Fixed());

		protected IRegistrations Next(IRegistrations next) => _current.Then(next);

		public IRegistration Include(Func<RelatedTypesHolster, IServiceTypes> related)
			=> Include(related(RelatedTypesHolster.Default));

		public IRegistration Include(IServiceTypes related) => _current.Get(related.Get(Services));

		public RegistrationResult Singleton() => Include(x => x.None).Singleton();

		public RegistrationResult Transient() => Include(x => x.None).Transient();

		public RegistrationResult Scoped() => Include(x => x.None).Scoped();
	}
}