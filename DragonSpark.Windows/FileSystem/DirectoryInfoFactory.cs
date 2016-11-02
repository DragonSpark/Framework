using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class DirectoryInfoFactory : FileSystemInfoFactory<DirectoryInfoBase, DirectoryInfo, IDirectoryInfo>
	{
		public static IConfigurableParameterizedSource<string, IDirectoryInfo> Default { get; } = new ConfigurableParameterizedSource<string, IDirectoryInfo>( o => new DirectoryInfoFactory().ToEqualityCache().Get );
		DirectoryInfoFactory() : base( DefaultImplementation.Implementation.Get ) {}

		public sealed class DefaultImplementation : ConfigurableParameterizedSource<string, DirectoryInfoBase>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( new FileSystemImplementationFactory<System.IO.DirectoryInfo, DirectoryInfoWrapper, DirectoryInfoBase>().Get ) {}
		}
	}
}