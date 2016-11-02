using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class DirectoryInfoFactory : FileSystemInfoFactory<DirectoryInfoBase, DirectoryInfo, IDirectoryInfo>
	{
		public static IParameterizedScope<string, IDirectoryInfo> Default { get; } = new ParameterizedScopedSingleton<string, IDirectoryInfo>( o => new DirectoryInfoFactory().ToEqualityCache().Get );
		DirectoryInfoFactory() : base( DefaultImplementation.Implementation.Get ) {}

		public sealed class DefaultImplementation : ParameterizedScopedSingleton<string, DirectoryInfoBase>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( new FileSystemImplementationFactory<System.IO.DirectoryInfo, DirectoryInfoWrapper, DirectoryInfoBase>().Get ) {}
		}
	}
}