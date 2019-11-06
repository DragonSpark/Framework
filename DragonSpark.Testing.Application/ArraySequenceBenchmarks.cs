using DragonSpark.Reflection;
using DragonSpark.Testing.Objects;

namespace DragonSpark.Testing.Application
{
	public class ArraySequenceBenchmarks : SequenceBenchmarks<uint>
	{
		public ArraySequenceBenchmarks() : base(I.A<ArrayEnumerations<uint>>().From) {}
	}
}