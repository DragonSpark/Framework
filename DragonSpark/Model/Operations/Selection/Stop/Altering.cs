using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class Altering<T> : StopAware<T, T>, IAltering<T>
{
	protected Altering(ISelect<Stop<T>, ValueTask<T>> select) : base(select) {}

	protected Altering(Func<Stop<T>, ValueTask<T>> select) : base(select) {}
}