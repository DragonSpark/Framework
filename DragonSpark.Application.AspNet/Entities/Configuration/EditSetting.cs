using DragonSpark.Application.AspNet.Entities.Editing;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

sealed class EditSetting : EditOrDefault<string, Setting>
{
	public EditSetting(IEnlistedScopes scope) : base(scope, SelectSetting.Default) {}
}