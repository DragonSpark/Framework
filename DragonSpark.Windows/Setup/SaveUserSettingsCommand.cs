using DragonSpark.Commands;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
	public sealed class SaveUserSettingsCommand : ConfigurableCommand<ApplicationSettingsBase>
	{
		public static SaveUserSettingsCommand Default { get; } = new SaveUserSettingsCommand();
		SaveUserSettingsCommand() : base( Implementation.DefaultImplementation.Execute ) {}

		sealed class Implementation : CommandBase<ApplicationSettingsBase>
		{
			public static Implementation DefaultImplementation { get; } = new Implementation();
			Implementation() : this( Defaults.UserSettingsPath, UserConfigurationLocator.Default.Get ) {}

			readonly Func<IFileInfo> fileSource;
			readonly Func<IFileInfo, System.Configuration.Configuration> configurationSource;

			[UsedImplicitly]
			public Implementation( Func<IFileInfo> fileSource, Func<IFileInfo, System.Configuration.Configuration> configurationSource )
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
}