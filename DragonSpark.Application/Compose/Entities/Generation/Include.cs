using Bogus;
using System;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public static class Include
	{
		public static Include<T, TOther> New<T, TOther>() where TOther : class
			=> new Include<T, TOther>((generator, _) => generator.Generate(), (_, _, _) => {},
			                          scope => scope.Once());

		public static IncludeMany<T, TOther> Many<T, TOther>() where TOther : class
			=> new IncludeMany<T, TOther>((generator, _) => generator.Generate(3), (_, _, _) => {});
	}

	public readonly struct Include<T, TOther> where TOther : class
	{
		readonly Func<Faker<TOther>, T, TOther> _generate;
		readonly Action<Faker, T, TOther>       _post;
		readonly AssignScope<T, TOther>         _scope;

		public Include(Func<Faker<TOther>, T, TOther> generate, Action<Faker, T, TOther> post,
		               AssignScope<T, TOther> scope)
		{
			_generate = generate;
			_post     = post;
			_scope    = scope;
		}

		public Include<T, TOther> Generate(Func<Faker<TOther>, T, TOther> generate)
			=> new Include<T, TOther>(generate, _post, _scope);

		public Include<T, TOther> Configure(Action<Faker, TOther> post)
			=> Configure((generator, _, instance) => post(generator, instance));

		public Include<T, TOther> Configure(Action<Faker, T, TOther> post)
			=> new Include<T, TOther>(_generate, post, _scope);

		public Include<T, TOther> Scoped(AssignScope<T, TOther> scope)
			=> new Include<T, TOther>(_generate, _post, scope);

		internal Payload Complete() => new Payload(_generate, _post, _scope);

		public readonly struct Payload
		{
			public Payload(Func<Faker<TOther>, T, TOther> generate, Action<Faker, T, TOther> configure,
			               AssignScope<T, TOther> scope)

			{
				Generate  = generate;
				Configure = configure;
				Scope     = scope;
			}

			public Func<Faker<TOther>, T, TOther> Generate { get; }

			public Action<Faker, T, TOther> Configure { get; }

			public AssignScope<T, TOther> Scope { get; }

			public Payload With(Action<Faker, T, TOther> configure) => new Payload(Generate, configure, Scope);

			public void Deconstruct(out Func<Faker<TOther>, T, TOther> generate, out Action<Faker, T, TOther> post,
			                        out AssignScope<T, TOther> scope)
			{
				generate = Generate;
				post     = Configure;
				scope    = Scope;
			}
		}
	}
}