using System.IO.Abstractions;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Windows.FileSystem
{
	sealed class Factory : ParameterConstructedCompositeFactory<FileSystemInfoBase, IFileSystemInfo>
	{
		public static Factory Default { get; } = new Factory();
		Factory() : base( typeof(DirectoryInfo), typeof(FileInfo) ) {}
	}
}