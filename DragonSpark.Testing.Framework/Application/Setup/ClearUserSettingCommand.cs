using DragonSpark.Commands;
using DragonSpark.Windows.FileSystem;
using Defaults = DragonSpark.Windows.Defaults;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class ClearUserSettingCommand : SuppliedCommand<IFileInfo>
	{
		public static ClearUserSettingCommand Default { get; } = new ClearUserSettingCommand();
		ClearUserSettingCommand() : base( DeleteFileCommand.Default, Defaults.UserSettingsPath ) {}
	}
}