using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Testing.Objects
{
	sealed class Data : Instance<string[]>
	{
		public static Data Default { get; } = new Data();

		Data() : base(FixtureInstance.Default.Many<string>(10_000).Select(x => x.Result()).Get()) {}
	}
}