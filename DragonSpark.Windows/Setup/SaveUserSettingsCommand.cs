using DragonSpark.Commands;
using System;
using System.Configuration;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Setup
{
	public sealed class SaveUserSettingsCommand : CommandBase<ApplicationSettingsBase>
	{
		public static SaveUserSettingsCommand Default { get; } = new SaveUserSettingsCommand();
		SaveUserSettingsCommand() : this( Defaults.UserSettingsPath, UserConfigurationLocator.Default.Get ) {}

		readonly Func<FileInfoBase> fileSource;
		readonly Func<FileInfoBase, System.Configuration.Configuration> configurationSource;

		SaveUserSettingsCommand( Func<FileInfoBase> fileSource, Func<FileInfoBase, System.Configuration.Configuration> configurationSource  )
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