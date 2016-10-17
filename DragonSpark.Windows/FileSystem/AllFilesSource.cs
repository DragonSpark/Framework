using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;
using System.IO;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class AllFilesSource : ParameterizedSourceBase<DirectoryInfoBase, ImmutableArray<FileInfoBase>>
	{
		public static AllFilesSource Default { get; } = new AllFilesSource();
		AllFilesSource() {}

		public override ImmutableArray<FileInfoBase> Get( DirectoryInfoBase parameter ) => parameter.GetFiles( "*.*", SearchOption.AllDirectories ).ToImmutableArray();
	}
}