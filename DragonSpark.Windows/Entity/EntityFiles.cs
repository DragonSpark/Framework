using System.Collections.Generic;
using System.IO;

namespace DragonSpark.Windows.Entity
{
	public static class EntityFiles
	{
		public static DirectoryInfo DefaultDataDirectory { get; } = new DirectoryInfo( @".\App_Data" );

		public static IEnumerable<FileInfo> WithLog( FileInfo databaseFile ) => new[] { databaseFile, GetLog( databaseFile ) };

		public static FileInfo GetLog( FileInfo database ) => new FileInfo( Path.Combine( database.DirectoryName ?? string.Empty, string.Concat( Path.GetFileNameWithoutExtension( database.Name ), "_log.ldf" ) ) );
	}
}