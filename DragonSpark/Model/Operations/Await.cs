using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Operations
{
	public delegate ConfiguredValueTaskAwaitable<TOut> Await<in TIn, TOut>(TIn parameter);
}