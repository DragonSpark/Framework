using System.IO.Abstractions;
using DragonSpark.Configuration;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class DirectoryFactory : ConfigurableParameterizedSource<string, IDirectoryInfo>
	{
		public static IConfigurableParameterizedSource<string, IDirectoryInfo> Default { get; } = new DirectoryFactory();
		DirectoryFactory() : base( s => new DirectoryInfo( new DirectoryInfoWrapper( new System.IO.DirectoryInfo( s ) ) ) ) {}
	}
}