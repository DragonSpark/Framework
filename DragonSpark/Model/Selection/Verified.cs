using DragonSpark.Compose;

namespace DragonSpark.Model.Selection;

sealed class Verified<_, T> : Select<_, T> where T : class
{
	public Verified(ISelect<_, T?> select) : base(select.Select(x => x.Verify())) {}
}