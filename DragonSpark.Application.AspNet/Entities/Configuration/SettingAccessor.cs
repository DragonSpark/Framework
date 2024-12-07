using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Configuration;

sealed class SettingAccessor : Operation<Pair<string, string?>>, ISettingAccessor
{
	readonly GetSetting _setting;

	public SettingAccessor(AssignSetting assign, GetSetting setting, RemoveSetting remove) : base(assign)
	{
		Remove   = remove;
		_setting = setting;
	}

	public IRemove Remove { get; }

	public async ValueTask<string?> Get(string parameter)
	{
		var setting = await _setting.Await(parameter);
		return setting?.Value;
	}
}