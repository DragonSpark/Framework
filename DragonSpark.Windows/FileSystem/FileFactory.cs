using DragonSpark.Configuration;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class FileFactory : ConfigurableParameterizedSource<string, IFileInfo>
	{
		public static IConfigurableParameterizedSource<string, IFileInfo> Default { get; } = new FileFactory();
		FileFactory() : base( s => new FileInfo( new FileInfoWrapper( new System.IO.FileInfo( s ) ) ) ) {}
	}
}