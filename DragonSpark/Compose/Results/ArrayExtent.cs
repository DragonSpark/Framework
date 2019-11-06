using DragonSpark.Model.Results;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Compose.Results
{
	public sealed class ArrayExtent<T> : Extent<T[]>
	{
		public static ArrayExtent<T> Default { get; } = new ArrayExtent<T>();

		ArrayExtent() {}

		public IResult<T[]> New(uint size) => New<int, T[]>.Default.In((int)size);
	}
}