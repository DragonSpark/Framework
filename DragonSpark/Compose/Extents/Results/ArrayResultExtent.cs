using DragonSpark.Compose.Model.Sequences;

namespace DragonSpark.Compose.Extents.Results;

public sealed class ArrayResultExtent<T> : ResultExtent<T[]>
{
	public static ArrayResultExtent<T> Default { get; } = new();

	ArrayResultExtent() : this(Start.A.Selection.Of.Type<int>().AndOf<T[]>().By.Instantiation.Get().Then()) {}

	readonly OpenArrayComposer<int, T> _select;

	public ArrayResultExtent(OpenArrayComposer<int, T> select) => _select = select;

	public Model.Results.ResultComposer<T[]> New(uint size) => _select.Subject.Bind((int)size);
}