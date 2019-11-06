using System.Collections.Generic;
using AutoFixture;
using DragonSpark.Model.Results;

namespace DragonSpark.Testing.Objects
{
	sealed class Strings : Instance<IEnumerable<string>>
	{
		public static Strings Default { get; } = new Strings();

		Strings() : base(new Fixture().CreateMany<string>()) {}
	}
}