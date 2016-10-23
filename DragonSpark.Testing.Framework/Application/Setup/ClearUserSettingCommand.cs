using DragonSpark.Commands;
using DragonSpark.Windows.FileSystem;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class ClearUserSettingCommand : SuppliedCommand<IFileInfo>
	{
		public static ClearUserSettingCommand Default { get; } = new ClearUserSettingCommand();
		ClearUserSettingCommand() : base( DeleteFileCommand.Default, Windows.Setup.Defaults.UserSettingsPath ) {}
	}
}