using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;
using System.IO;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class AllFilesSource : ParameterizedSourceBase<IDirectoryInfo, ImmutableArray<IFileInfo>>
	{
		public static AllFilesSource Default { get; } = new AllFilesSource();
		AllFilesSource() {}

		public override ImmutableArray<IFileInfo> Get( IDirectoryInfo parameter ) => parameter.GetFiles( "*.*", SearchOption.AllDirectories ).ToImmutableArray();
	}
}