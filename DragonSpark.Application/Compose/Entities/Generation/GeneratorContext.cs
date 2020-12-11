using Bogus;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public sealed class GeneratorContext<T> : IResult<T> where T : class
	{
		public static implicit operator T(GeneratorContext<T> instance) => instance.Get();

		readonly Faker<T> _subject;

		public GeneratorContext() : this(Generator<T>.Default.Get()) {}

		public GeneratorContext(Faker<T> subject) => _subject = subject;

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property) where TOther : class
			=> Include(property, (faker, other) => {});

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property, Action<Faker, TOther> post)
			where TOther : class
		{
			var configured = _subject.RuleFor(property, new Rule<T, TOther>(post).Get);
			var result     = new GeneratorContext<T>(configured);
			return result;
		}

		public T Get() => _subject.Generate();
	}
}