using DragonSpark.Application.Entities.Editing;

namespace DragonSpark.Application.Entities.Configuration;

sealed class EditSetting : EditOrDefault<string, Setting>
{
	public EditSetting(IEnlistedScopes scope) : base(scope, SelectSetting.Default) {}
}