using Bogus;
using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class ManyRule<T, TOther> : IRule<T, List<TOther>> where TOther : class
	{
		readonly ISelect<T, List<TOther>>       _generate;
		readonly Action<Faker, T, List<TOther>> _post;

		public ManyRule(ISelect<T, List<TOther>> generate, Action<Faker, T, List<TOther>> post)
		{
			_generate = generate;
			_post     = post;
		}

		public List<TOther> Get((Faker, T) parameter)
		{
			var (faker, owner) = parameter;
			var result = _generate.Get(owner);
			_post(faker, owner, result);
			return result;
		}
	}
}