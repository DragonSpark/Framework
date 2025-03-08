using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

sealed class Read<T> : ISelecting<string, T?>
{
	readonly IDistributedCache   _store;
	readonly ISelect<string, T?> _deserialize;

	public Read(IDistributedCache store) : this(store, Deserialize<T>.Default) {}

	public Read(IDistributedCache store, ISelect<string, T?> deserialize)
	{
		_store       = store;
		_deserialize = deserialize;
	}

	public async ValueTask<T?> Get(string parameter)
	{
		var content = await _store.GetStringAsync(parameter).Off();
		return content is not null ? _deserialize.Get(content) : default;
	}
}