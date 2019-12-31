using DragonSpark.Compose;
using DragonSpark.Testing.Objects;

namespace DragonSpark.Testing.Application
{
	public class ArraySequenceBenchmarks : SequenceBenchmarks<uint>
	{
		public ArraySequenceBenchmarks() : base(Start.An.Extent<ArrayEnumerations<uint>>().From) {}
	}
}