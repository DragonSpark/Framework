using DragonSpark.Commands;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Setup
{
	public sealed class ClearUserSettingCommand : SuppliedCommand<FileInfoBase>
	{
		public static ClearUserSettingCommand Default { get; } = new ClearUserSettingCommand();
		ClearUserSettingCommand() : base( DeleteFileCommand.Default, Defaults.UserSettingsPath ) {}
	}
}