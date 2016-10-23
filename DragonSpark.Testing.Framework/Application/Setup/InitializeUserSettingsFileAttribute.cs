using DragonSpark.Sources;
using DragonSpark.Testing.Framework.FileSystem;
using JetBrains.Annotations;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class InitializeUserSettingsFileAttribute : RegisterFilesAttribute
	{
		public InitializeUserSettingsFileAttribute() : this( UserSettingsFilePath.Current.GetCurrent() ) {}

		[UsedImplicitly]
		public InitializeUserSettingsFileAttribute( string userSettingsFilePath ) : base( userSettingsFilePath ) {}
	}
}