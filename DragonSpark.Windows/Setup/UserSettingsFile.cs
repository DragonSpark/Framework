using DragonSpark.Aspects;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.FileSystem;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
	public sealed class UserSettingsFile : DelegatedParameterizedSource<ConfigurationUserLevel, IFileInfo>
	{
		public static UserSettingsFile Default { get; } = new UserSettingsFile();
		UserSettingsFile() : base( Inner.DefaultNested.Get ) {}

		public override IFileInfo Get( ConfigurationUserLevel parameter ) => base.Get( parameter ).Refreshed();

		sealed class Inner : ParameterizedSourceBase<ConfigurationUserLevel, IFileInfo>
		{
			public static Inner DefaultNested { get; } = new Inner();
			Inner() {}

			[Freeze]
			public override IFileInfo Get( ConfigurationUserLevel parameter ) => 
				FileFactory.Default.Get( ConfigurationManager.OpenExeConfiguration( parameter ).FilePath );
		}
	}
}