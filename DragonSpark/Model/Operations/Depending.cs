using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Depending<T> : Selecting<T, bool>, IDepending<T>
{
	public Depending(ISelect<T, ValueTask<bool>> select) : base(select) {}

	public Depending(Func<T, ValueTask<bool>> select) : base(select) {}
}

public class Depending : Depending<None>, IDepending
{
	public Depending(IResult<ValueTask<bool>> select) : this(select.Then().Accept()) {}

	public Depending(Func<ValueTask<bool>> select) : this(select.Start().Accept()) {}

	public Depending(ISelect<None, ValueTask<bool>> select) : base(select) {}

	public Depending(Func<None, ValueTask<bool>> select) : base(select) {}
}