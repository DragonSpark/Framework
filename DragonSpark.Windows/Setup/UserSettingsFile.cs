using DragonSpark.Aspects;
using DragonSpark.Sources.Parameterized;
using System.Configuration;
using System.IO;

namespace DragonSpark.Windows.Setup
{
	public sealed class UserSettingsFile : DelegatedParameterizedSource<ConfigurationUserLevel, FileInfo>
	{
		public static UserSettingsFile Default { get; } = new UserSettingsFile();
		UserSettingsFile() : base( Inner.DefaultNested.Get ) {}

		public override FileInfo Get( ConfigurationUserLevel parameter ) => base.Get( parameter ).Refreshed();

		sealed class Inner : ParameterizedSourceBase<ConfigurationUserLevel, FileInfo>
		{
			public static Inner DefaultNested { get; } = new Inner();
			Inner() {}

			[Freeze]
			public override FileInfo Get( ConfigurationUserLevel parameter ) => 
				new FileInfo( ConfigurationManager.OpenExeConfiguration( parameter ).FilePath );
		}
	}
}