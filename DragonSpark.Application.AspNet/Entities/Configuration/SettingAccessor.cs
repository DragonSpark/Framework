using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

sealed class SettingAccessor : StopAware<Pair<string, string?>>, ISettingAccessor
{
	readonly GetSetting _setting;

	public SettingAccessor(AssignSetting assign, GetSetting setting, RemoveSetting remove) : base(assign)
	{
		Remove   = remove;
		_setting = setting;
	}

	public IRemove Remove { get; }

	public async ValueTask<string?> Get(Stop<string> parameter)
	{
		var setting = await _setting.Off(parameter);
		return setting?.Value;
	}
}