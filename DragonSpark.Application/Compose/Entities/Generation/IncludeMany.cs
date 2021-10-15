using Bogus;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Entities.Generation;

public readonly struct IncludeMany<T, TOther> where TOther : class
{
	readonly Func<Faker<TOther>, T, List<TOther>> _generate;
	readonly Action<Faker, T, List<TOther>>       _post;

	public IncludeMany(Func<Faker<TOther>, T, List<TOther>> generate, Action<Faker, T, List<TOther>> post)
	{
		_generate = generate;
		_post     = post;
	}

	public IncludeMany<T, TOther> Generate(Func<Faker<TOther>, T, List<TOther>> generate)
		=> new IncludeMany<T, TOther>(generate, _post);

	public IncludeMany<T, TOther> Configure(Action<Faker, List<TOther>> post)
		=> Configure((generator, _, instance) => post(generator, instance));

	public IncludeMany<T, TOther> Configure(Action<Faker, T, List<TOther>> post)
		=> new IncludeMany<T, TOther>(_generate, post);

	internal Payload Complete() => new Payload(_generate, _post);

	public readonly struct Payload
	{
		public Payload(Func<Faker<TOther>, T, List<TOther>> generate, Action<Faker, T, List<TOther>> configure)

		{
			Generate  = generate;
			Configure = configure;
		}

		public Func<Faker<TOther>, T, List<TOther>> Generate { get; }

		public Action<Faker, T, List<TOther>> Configure { get; }

		public Payload With(Action<Faker, T, List<TOther>> configure) => new Payload(Generate, configure);

		public void Deconstruct(out Func<Faker<TOther>, T, List<TOther>> generate,
		                        out Action<Faker, T, List<TOther>> post)
		{
			generate = Generate;
			post     = Configure;
		}
	}
}