using DragonSpark.Configuration;
using DragonSpark.Sources.Parameterized;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class FileInfoFactory : FileSystemInfoFactory<FileInfoBase, FileInfo, IFileInfo>
	{
		public static IConfigurableParameterizedSource<string, IFileInfo> Default { get; } = new FileInfoFactory().ToEqualityCache().AsConfigurable();
		FileInfoFactory() : base( Implementation.DefaultImplementation.Get ) {}

		public sealed class Implementation : ConfigurableParameterizedSource<string, FileInfoBase>
		{
			public static Implementation DefaultImplementation { get; } = new Implementation();
			Implementation() : base( new FileSystemImplementationFactory<System.IO.FileInfo, FileInfoWrapper, FileInfoBase>().ToEqualityCache().Get ) {}
		}
	}
}