using AutoFixture;
using DragonSpark.Model.Results;

namespace DragonSpark.Testing.Objects
{
	sealed class FixtureInstance : Instance<IFixture>
	{
		public static FixtureInstance Default { get; } = new FixtureInstance();

		FixtureInstance() : base(new Fixture()) {}
	}
}