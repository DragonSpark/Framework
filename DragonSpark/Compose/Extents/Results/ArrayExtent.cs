using DragonSpark.Compose.Model;
using DragonSpark.Model.Results;

namespace DragonSpark.Compose.Extents.Results
{
	public sealed class ArrayExtent<T> : Extent<T[]>
	{
		public static ArrayExtent<T> Default { get; } = new ArrayExtent<T>();

		ArrayExtent() : this(Start.A.Selection.Of.Type<int>().AndOf<T[]>().By.Instantiation.Then()) {}

		readonly OpenArraySelector<int, T> _select;

		public ArrayExtent(OpenArraySelector<int, T> select) => _select = select;

		public IResult<T[]> New(uint size) => _select.Bind((int)size).Cast<T[]>().Get();
	}
}