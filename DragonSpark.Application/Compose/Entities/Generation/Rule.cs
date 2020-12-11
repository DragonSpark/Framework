using Bogus;
using System;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	sealed class Rule<T, TOther> : IRule<T, TOther> where TOther : class
	{
		readonly Func<Faker<TOther>, T, TOther> _generate;
		readonly Action<Faker, T, TOther>       _post;
		readonly Faker<TOther>                  _generator;

		public Rule(Faker<TOther> generator, Func<Faker<TOther>, T, TOther> generate, Action<Faker, T, TOther> post)
		{
			_generate  = generate;
			_post      = post;
			_generator = generator;
		}

		public TOther Get((Faker, T) parameter)
		{
			var (faker, owner) = parameter;
			var result = _generate(_generator, owner);
			_post(faker, owner, result);
			return result;
		}
	}
}