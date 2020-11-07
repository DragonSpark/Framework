using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public delegate ValueTask<TOut> Operate<in TIn, TOut>(TIn parameter);

	public delegate ValueTask Operate<in T>(T parameter);

	public delegate ValueTask Operate();

	public delegate ConfiguredValueTaskAwaitable Await<in T>(T parameter);

	public delegate ConfiguredValueTaskAwaitable Await();
}