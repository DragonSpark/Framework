using Bogus;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.Entities.Generation;

sealed class Generate<T, TOther> : ISelect<T, TOther> where TOther : class
{
	readonly Faker<TOther>                  _generator;
	readonly Func<Faker<TOther>, T, TOther> _generate;

	public Generate(Faker<TOther> generator, Func<Faker<TOther>, T, TOther> generate)
	{
		_generator = generator;
		_generate  = generate;
	}

	public TOther Get(T parameter) => _generate(_generator, parameter);
}