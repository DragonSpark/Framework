using DragonSpark.Application.Entities.Editing;

namespace DragonSpark.Application.Entities.Configuration;

sealed class EditSetting : EditOrDefault<string, Setting>
{
	public EditSetting(IEnlistedScopes context) : base(context, SelectSetting.Default) {}
}