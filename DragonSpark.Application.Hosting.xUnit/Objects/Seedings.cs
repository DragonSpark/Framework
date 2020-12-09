using Bogus;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Hosting.xUnit.Objects
{
	sealed class Seedings : ISelect<IFakerTInternal, uint>
	{
		public static Seedings Default { get; } = new Seedings();

		Seedings() {}

		public uint Get(IFakerTInternal parameter)
			=> parameter.To<IFakerTInternal>().LocalSeed?.Grade() ?? Randomizer.Seed.Next().Grade();
	}
}