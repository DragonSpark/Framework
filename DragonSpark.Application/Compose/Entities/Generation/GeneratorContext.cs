using Bogus;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
			=> IncludeAndLocateAssignment(property, (generator, _) => generator.Generate(), (_, __, ___) => {});

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
			=> IncludeAndLocateAssignment(property, generate, (generator, _, instance) => post(generator, instance));

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property, Action<Faker, T, TOther> post)
			where TOther : class => IncludeAndLocateAssignment(property, (generator, _) => generator.Generate(), post);

		public GeneratorContext<T> IncludeAndLocateAssignment<TOther>(Expression<Func<T, TOther>> property,
		                                                              Func<Faker<TOther>, T, TOther> generate,
		                                                              Action<Faker, T, TOther> post)
			where TOther : class
		{
			var assignment = LocateAssignment<TOther, T>.Default.Get();
			var configure  = assignment != null ? new Assign<T, TOther>(post, assignment).Execute : post;
			var result     = Include(property, generate, configure);
			return result;
		}

		// ReSharper disable once TooManyArguments
		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property,
		                                           Expression<Func<TOther, T>> other)
			where TOther : class
			=> Include(property, other, (_, __, ___) => {});

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property,
		                                           Expression<Func<TOther, T>> other,
		                                           Action<Faker, T, TOther> post)
			where TOther : class
			=> Include(property, other, (generator, _) => generator.Generate(), post);

		// ReSharper disable once TooManyArguments
		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property,
		                                           Expression<Func<TOther, T>> other,
		                                           Func<Faker<TOther>, T, TOther> generate)
			where TOther : class
			=> Include(property, other, generate, (_, __, ___) => {});

		// ReSharper disable once TooManyArguments
		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property,
		                                           Expression<Func<TOther, T>> other,
		                                           Func<Faker<TOther>, T, TOther> generate,
		                                           Action<Faker, T, TOther> post)
			where TOther : class
		{
			var action = LocateAssignments<TOther, T>.Default.Get(other.GetMemberAccess().Name)
			                                         .Get()
			                                         .Verify($"The expression '{other}' did not resolve to a valid assignment setter.");
			var result = Include(property, generate, new Assign<T, TOther>(post, action).Execute);
			return result;
		}

		public GeneratorContext<T> Include<TOther>(Expression<Func<T, TOther>> property,
		                                           Func<Faker<TOther>, T, TOther> generate,
		                                           Action<Faker, T, TOther> post)
			where TOther : class
		{
			var configured = _subject.RuleFor(property, new Rule<T, TOther>(generate, post).Get);
			var result     = new GeneratorContext<T>(configured);
			return result;
		}

		public T Get() => _subject.Generate();
	}
}