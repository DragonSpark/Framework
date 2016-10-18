using DragonSpark.Commands;
using DragonSpark.Windows.FileSystem;

namespace DragonSpark.Windows.Setup
{
	public sealed class ClearUserSettingCommand : SuppliedCommand<IFileInfo>
	{
		public static ClearUserSettingCommand Default { get; } = new ClearUserSettingCommand();
		ClearUserSettingCommand() : base( DeleteFileCommand.Default, Defaults.UserSettingsPath ) {}
	}
}