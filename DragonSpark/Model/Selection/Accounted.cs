namespace DragonSpark.Model.Selection;

sealed class Accounted<_, T> : ISelect<_?, T>
{
	readonly ISelect<_, T> _select;

	public Accounted(ISelect<_, T> select) => _select = select;

	public T Get(_? parameter) => _select.Get(parameter!);
}