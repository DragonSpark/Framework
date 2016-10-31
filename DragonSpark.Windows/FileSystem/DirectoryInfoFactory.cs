using DragonSpark.Configuration;
using DragonSpark.Sources.Parameterized;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class DirectoryInfoFactory : FileSystemInfoFactory<DirectoryInfoBase, DirectoryInfo, IDirectoryInfo>
	{
		public static IConfigurableParameterizedSource<string, IDirectoryInfo> Default { get; } = new DirectoryInfoFactory().ToEqualityCache().AsConfigurable();
		DirectoryInfoFactory() : base( DefaultImplementation.Implementation.Get ) {}

		public sealed class DefaultImplementation : ConfigurableParameterizedSource<string, DirectoryInfoBase>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( new FileSystemImplementationFactory<System.IO.DirectoryInfo, DirectoryInfoWrapper, DirectoryInfoBase>().ToEqualityCache().Get ) {}
		}
	}
}