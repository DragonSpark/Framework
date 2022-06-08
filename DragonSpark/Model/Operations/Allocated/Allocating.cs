using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class Allocating<T, TOut> : Select<T, Task<TOut>>, IAllocating<T, TOut>
{
	public Allocating(ISelect<T, Task<TOut>> @select) : base(@select) {}

	public Allocating(Func<T, Task<TOut>> @select) : base(@select) {}
}