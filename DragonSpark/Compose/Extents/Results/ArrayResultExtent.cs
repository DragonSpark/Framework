using DragonSpark.Compose.Model;

namespace DragonSpark.Compose.Extents.Results
{
	public sealed class ArrayResultExtent<T> : ResultExtent<T[]>
	{
		public static ArrayResultExtent<T> Default { get; } = new ArrayResultExtent<T>();

		ArrayResultExtent() : this(Start.A.Selection.Of.Type<int>().AndOf<T[]>().By.Instantiation.Then()) {}

		readonly OpenArraySelector<int, T> _select;

		public ArrayResultExtent(OpenArraySelector<int, T> select) => _select = select;

		public Model.ResultContext<T[]> New(uint size) => _select.Bind((int)size).Cast<T[]>().Get().Then(); // TODO: Remove Cast.
	}
}