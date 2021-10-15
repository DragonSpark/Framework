using Bogus;
using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Generation;

sealed class GenerateList<T, TOther> : ISelect<T, List<TOther>> where TOther : class
{
	readonly Faker<TOther>                        _generator;
	readonly Func<Faker<TOther>, T, List<TOther>> _generate;

	public GenerateList(Faker<TOther> generator, Func<Faker<TOther>, T, List<TOther>> generate)
	{
		_generator = generator;
		_generate  = generate;
	}

	public List<TOther> Get(T parameter) => _generate(_generator, parameter);
}