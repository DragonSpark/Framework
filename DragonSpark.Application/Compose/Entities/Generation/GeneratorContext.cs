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
			=> Include(property, (generator, _) => generator.Generate(), (_, __, ___) => {});

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property, Action<Faker, TOther> post)
			where TOther : class
			=> Include(property, (generator, _) => generator.Generate(), post);

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property,
		                                           Func<Faker<TOther>, T, TOther> generate)
			where TOther : class
			=> Include(property, generate, (faker, other) => {});

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property,
		                                           Func<Faker<TOther>, T, TOther> generate, Action<Faker, TOther> post)
			where TOther : class
			=> Include(property, generate, (generator, _, instance) => post(generator, instance));

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property,
		                                           Action<Faker, T, TOther> post)
			where TOther : class => Include(property, (generator, _) => generator.Generate(), post);

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property,
		                                           Func<Faker<TOther>, T, TOther> generate,
		                                           Action<Faker, T, TOther> post)
			where TOther : class
		{
			var assignment = LocateAssignment<TOther, T>.Default.Get();
			var configure  = assignment != null ? new Assign<T, TOther>(post, assignment).Execute : post;

			var configured = _subject.RuleFor(property, new Rule<T, TOther>(generate, configure).Get);
			var result     = new GeneratorContext<T>(configured);
			return result;
		}

		public T Get() => _subject.Generate();
	}
}