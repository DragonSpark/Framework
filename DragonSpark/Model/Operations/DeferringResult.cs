using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class DeferringResult<T> : IResulting<T>
{
	readonly IMutable<T?> _store;
	readonly AwaitOf<T>   _result;

	public DeferringResult(IResulting<T> result) : this(new Variable<T?>(), result) {}

	public DeferringResult(IMutable<T?> store, IResulting<T> result) : this(store, result.Await) {}

	public DeferringResult(AwaitOf<T> result) : this(new Variable<T?>(), result) {}

	public DeferringResult(IMutable<T?> store, AwaitOf<T> result)
	{
		_store  = store;
		_result = result;
	}

	public async ValueTask<T> Get()
	{
		var current = _store.Get();
		if (current is null)
		{
			var result = await _result();
			_store.Execute(result);
			return result;
		}

		return current;
	}
}