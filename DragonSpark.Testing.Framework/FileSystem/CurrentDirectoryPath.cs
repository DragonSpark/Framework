using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class CurrentDirectoryPath : Scope<string>
	{
		public static CurrentDirectoryPath Default { get; } = new CurrentDirectoryPath();
		CurrentDirectoryPath() : base( Windows.FileSystem.Defaults.CurrentPath.Wrap() ) {}
	}
}