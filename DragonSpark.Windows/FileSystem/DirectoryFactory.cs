using DragonSpark.Configuration;
using DragonSpark.Sources.Parameterized;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class DirectoryFactory : FileSystemInfoFactory<DirectoryInfoBase, IDirectoryInfo>
	{
		public static IConfigurableParameterizedSource<string, IDirectoryInfo> Default { get; } = new DirectoryFactory().ToEqualityCache().AsConfigurable();
		DirectoryFactory() : base( Implementation.DefaultImplementation.Get ) {}

		public sealed class Implementation : ConfigurableParameterizedSource<string, DirectoryInfoBase>
		{
			public static Implementation DefaultImplementation { get; } = new Implementation();
			Implementation() : base( new FileSystemImplementationFactory<System.IO.DirectoryInfo, DirectoryInfoWrapper, DirectoryInfoBase>().ToEqualityCache().Get ) {}
		}
	}
}