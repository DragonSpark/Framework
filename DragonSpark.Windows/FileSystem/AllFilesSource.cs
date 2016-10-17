using System.Collections.Immutable;
using System.IO;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class AllFilesSource : ParameterizedSourceBase<DirectoryInfo, ImmutableArray<FileInfo>>
	{
		public static AllFilesSource Default { get; } = new AllFilesSource();
		AllFilesSource() {}

		public override ImmutableArray<FileInfo> Get( DirectoryInfo parameter ) => parameter.GetFiles( "*.*", SearchOption.AllDirectories ).ToImmutableArray();
	}
}