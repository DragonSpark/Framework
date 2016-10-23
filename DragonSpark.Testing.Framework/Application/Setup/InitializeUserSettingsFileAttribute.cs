using DragonSpark.Sources;
using DragonSpark.Testing.Framework.FileSystem;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class InitializeUserSettingsFileAttribute : RegisterFilesAttribute
	{
		public InitializeUserSettingsFileAttribute() : this( UserSettingsFilePath.Current.GetCurrent() ) {}

		public InitializeUserSettingsFileAttribute( string userSettingsFilePath ) : base( userSettingsFilePath ) {}
	}
}