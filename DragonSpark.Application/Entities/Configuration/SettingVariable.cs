using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Configuration;

public class SettingVariable : ISettingVariable
{
	readonly string           _key;
	readonly ISettingAccessor _store;

	protected SettingVariable(string key, ISettingAccessor store)
		: this(key, store, store.Remove.Then().Bind(key).Out()) {}

	protected SettingVariable(string key, ISettingAccessor store, IOperation remove)
	{
		_key   = key;
		_store = store;
		Remove = remove;
	}

	public async ValueTask<string?> Get()
	{
		var result = await _store.Get(_key).ConfigureAwait(false);
		return result;
	}

	public ValueTask Get(string parameter) => _store.Get((_key, parameter));

	public IOperation Remove { get; }
}