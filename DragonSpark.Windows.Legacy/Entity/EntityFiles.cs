using DragonSpark.Windows.FileSystem;
using System.Collections.Generic;
using Path = DragonSpark.Windows.FileSystem.Path;

namespace DragonSpark.Windows.Legacy.Entity
{
	public static class EntityFiles
	{
		public static IDirectoryInfo DefaultDataDirectory { get; } = DirectoryInfoFactory.Default.Get( @".\App_Data" );

		public static IEnumerable<IFileInfo> WithLog( IFileInfo databaseFile ) => new[] { databaseFile, GetLog( databaseFile ) };

		public static IFileInfo GetLog( IFileInfo database )
		{
			var path = Path.Default.Get();
			var result = FileInfoFactory.Default.Get( path.Combine( database.DirectoryName ?? string.Empty, string.Concat( path.GetFileNameWithoutExtension( database.Name ), "_log.ldf" ) ) );
			return result;
		}
	}
}