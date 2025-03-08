using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

public class Storing<T> : IResulting<T>
{
	readonly IMutationAware<T?> _store;
	readonly AwaitOf<T>        _source;

	public Storing(IResult<ValueTask<T>> source) : this(new Variable<T>(), source) {}

	public Storing(IMutable<T?> mutable, IResult<ValueTask<T>> source)
		: this(new AssignedAwareVariable<T>(mutable), source) {}

	public Storing(IMutationAware<T?> store, IResult<ValueTask<T>> source) : this(store, source.Off) {}

	public Storing(IMutationAware<T?> store, AwaitOf<T> source)
	{
		_store  = store;
		_source = source;
	}

	public async ValueTask<T> Get()
	{
		if (_store.IsSatisfiedBy())
		{
			return _store.Get().Verify();
		}

		var result = await _source();
		_store.Execute(result);
		return result;
	}
}