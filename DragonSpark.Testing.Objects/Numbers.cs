using DragonSpark.Compose;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Testing.Objects
{
	sealed class Numbers : ArrayStore<uint, int>
	{
		public static Numbers Default { get; } = new Numbers();

		Numbers() : base(Start.An.Extent<ClassicTake<int>>().From(AllNumbers.Default.ToDelegate())) {}
	}
}