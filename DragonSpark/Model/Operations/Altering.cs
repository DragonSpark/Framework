using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Altering<T> : Selecting<T, T>
{
	public Altering(ISelect<T, ValueTask<T>> @select) : base(@select) {}

	public Altering(Func<T, ValueTask<T>> @select) : base(@select) {}
}