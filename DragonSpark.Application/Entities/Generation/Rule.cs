using Bogus;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class Rule<T, TOther> : IRule<T, TOther> where TOther : class
	{
		readonly ISelect<T, TOther>       _generate;
		readonly Action<Faker, T, TOther> _post;

		public Rule(ISelect<T, TOther> generate, Action<Faker, T, TOther> post)
		{
			_generate = generate;
			_post     = post;
		}

		public TOther Get((Faker, T) parameter)
		{
			var (faker, owner) = parameter;
			var result = _generate.Get(owner);
			_post(faker, owner, result);
			return result;
		}
	}
}