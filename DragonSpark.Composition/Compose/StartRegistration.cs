using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	public sealed class StartRegistration<T> : IIncludingRegistration where T : class
	{
		readonly IServiceCollection _subject;

		public StartRegistration(IServiceCollection subject) => _subject = subject;

		IRegistrations Current => new Register<T>(_subject).Adapt().Fixed();

		IRegistrations Expanded => new Registrations<T>(_subject).Then(Current);

		public CompositeRegistration And<TNext>() where TNext : class
			=> new CompositeRegistration(_subject,
			                             Expanded.Then(new Registrations<TNext>(_subject)
				                                           .Then(new Register<TNext>(_subject))));

		public Registration<T> Forward<TTo>() where TTo : class, T
			=> new Registration<T>(_subject,
			                       new Registrations<TTo>(_subject).Then(new Forward<T, TTo>(_subject)));

		public Registration<T> Use<TResult>() where TResult : class, IResult<T>
			=> new Registration<T>(_subject,
			                       new Registrations<TResult>(_subject)
				                       .Then(new ResultRegistration<T, TResult>(_subject)));

		public Registration<T> UseEnvironment()
			=> new Registration<T>(_subject, new SelectedRegistration<T>(_subject));

		public IRegistration Include(Func<RelatedTypesHolster, IServiceTypes> related)
			=> Include(related(RelatedTypesHolster.Default));

		public IRegistration Include(IServiceTypes related) => Expanded.Get(related.Get(_subject));

		public RegistrationResult Singleton() => Include(x => x.None).Singleton();

		public RegistrationResult Transient() => Include(x => x.None).Transient();

		public RegistrationResult Scoped() => Include(x => x.None).Scoped();
	}
}