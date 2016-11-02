using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class FileInfoFactory : FileSystemInfoFactory<FileInfoBase, FileInfo, IFileInfo>
	{
		public static IConfigurableParameterizedSource<string, IFileInfo> Default { get; } = new ConfigurableParameterizedSource<string, IFileInfo>( o => new FileInfoFactory().ToEqualityCache().Get );
		FileInfoFactory() : base( DefaultImplementation.Implementation.Get ) {}

		public sealed class DefaultImplementation : ConfigurableParameterizedSource<string, FileInfoBase>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( new FileSystemImplementationFactory<System.IO.FileInfo, FileInfoWrapper, FileInfoBase>().Get ) {}
		}
	}
}