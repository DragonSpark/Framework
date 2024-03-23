using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

[UsedImplicitly]
public class Altering<T> : Selecting<T, T>, IAltering<T>
{
	public Altering(ISelect<T, ValueTask<T>> @select) : base(@select) {}

	public Altering(Func<T, ValueTask<T>> @select) : base(@select) {}
}