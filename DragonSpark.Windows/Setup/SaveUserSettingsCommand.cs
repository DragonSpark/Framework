using System;
using System.Configuration;
using System.IO;
using DragonSpark.Commands;

namespace DragonSpark.Windows.Setup
{
	public sealed class SaveUserSettingsCommand : CommandBase<ApplicationSettingsBase>
	{
		public static SaveUserSettingsCommand Default { get; } = new SaveUserSettingsCommand();
		SaveUserSettingsCommand() : this( Defaults.UserSettingsPath, UserConfigurationLocator.Default.Get ) {}

		readonly Func<FileInfo> fileSource;
		readonly Func<FileInfo, System.Configuration.Configuration> configurationSource;

		SaveUserSettingsCommand( Func<FileInfo> fileSource, Func<FileInfo, System.Configuration.Configuration> configurationSource  )
		{
			this.fileSource = fileSource;
			this.configurationSource = configurationSource;
		}

		public override void Execute( ApplicationSettingsBase parameter )
		{
			var file = fileSource();
			var configuration = configurationSource( file ) ?? ConfigurationManager.OpenExeConfiguration( parameter.GetType().Assembly.Location );
			configuration.SaveAs( file.FullName, ConfigurationSaveMode.Modified );
			parameter.Save();
		}
	}
}