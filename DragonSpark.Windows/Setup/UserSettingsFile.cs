using DragonSpark.Aspects;
using DragonSpark.Sources.Parameterized;
using System.Configuration;
using System.IO;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Setup
{
	public sealed class UserSettingsFile : DelegatedParameterizedSource<ConfigurationUserLevel, FileInfoBase>
	{
		public static UserSettingsFile Default { get; } = new UserSettingsFile();
		UserSettingsFile() : base( Inner.DefaultNested.Get ) {}

		public override FileInfoBase Get( ConfigurationUserLevel parameter ) => base.Get( parameter ).Refreshed();

		sealed class Inner : ParameterizedSourceBase<ConfigurationUserLevel, FileInfoBase>
		{
			public static Inner DefaultNested { get; } = new Inner();
			Inner() {}

			[Freeze]
			public override FileInfoBase Get( ConfigurationUserLevel parameter ) => new FileInfo( ConfigurationManager.OpenExeConfiguration( parameter ).FilePath );
		}
	}
}