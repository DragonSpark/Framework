using DragonSpark.Windows.FileSystem;
using System.Collections.Generic;
using Path = DragonSpark.Windows.FileSystem.Path;

namespace DragonSpark.Windows.Legacy.Entity
{
	public static class EntityFiles
	{
		public static IDirectoryInfo DefaultDataDirectory { get; } = DirectoryFactory.Default.Get( @".\App_Data" );

		public static IEnumerable<IFileInfo> WithLog( IFileInfo databaseFile ) => new[] { databaseFile, GetLog( databaseFile ) };

		public static IFileInfo GetLog( IFileInfo database )
		{
			var path = Path.Current.Get();
			var result = FileFactory.Default.Get( path.Combine( database.DirectoryName ?? string.Empty, string.Concat( path.GetFileNameWithoutExtension( database.Name ), "_log.ldf" ) ) );
			return result;
		}
	}
}