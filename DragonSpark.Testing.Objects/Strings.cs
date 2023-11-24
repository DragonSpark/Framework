using AutoFixture;
using DragonSpark.Model.Results;
using System.Collections.Generic;

namespace DragonSpark.Testing.Objects;

sealed class Strings : Instance<IEnumerable<string>>
{
	public static Strings Default { get; } = new();

	Strings() : base(new Fixture().CreateMany<string>()) {}
}