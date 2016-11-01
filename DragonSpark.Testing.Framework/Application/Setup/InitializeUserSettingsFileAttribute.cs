using DragonSpark.Testing.Framework.FileSystem;
using JetBrains.Annotations;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class InitializeUserSettingsFileAttribute : RegisterFilesAttribute
	{
		public InitializeUserSettingsFileAttribute() : this( UserSettingsFilePath.Default.Get() ) {}

		[UsedImplicitly]
		public InitializeUserSettingsFileAttribute( string userSettingsFilePath ) : base( userSettingsFilePath ) {}
	}
}