namespace DragonSpark.Model.Selection;

sealed class AccountedStructure<_, T> : ISelect<_?, T> where _ : struct
{
	readonly ISelect<_, T> _select;

	public AccountedStructure(ISelect<_, T> select) => _select = select;

	public T Get(_? parameter) => _select.Get(parameter.GetValueOrDefault());
}