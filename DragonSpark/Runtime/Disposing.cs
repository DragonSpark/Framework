using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Runtime;

public class Disposing : IAsyncDisposable
{
	readonly Operate _operate;

	protected Disposing(Operate operate) => _operate = operate;

	public ValueTask DisposeAsync() => _operate();
}