using DragonSpark.Commands;
using System.IO;

namespace DragonSpark.Windows.Setup
{
	public sealed class ClearUserSettingCommand : SuppliedCommand<FileInfo>
	{
		public static ClearUserSettingCommand Default { get; } = new ClearUserSettingCommand();
		ClearUserSettingCommand() : base( DeleteFileCommand.Default, Defaults.UserSettingsPath ) {}
	}
}