using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Runtime;

public class Disposing : IAsyncDisposable
{
	readonly Operate _operate;

	public Disposing(Operate operate) => _operate = operate;

	public ValueTask DisposeAsync() => _operate();
}