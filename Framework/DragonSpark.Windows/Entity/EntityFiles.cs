using System.Collections.Generic;
using System.IO;

namespace DragonSpark.Windows.Entity
{
	public static class EntityFiles
	{
		public static DirectoryInfo DefaultDataDirectory { get; } = new DirectoryInfo( @".\App_Data" );

		public static IEnumerable<FileInfo> WithLog( FileInfo databaseFile )
		{
			var result = new[] { databaseFile, GetLog( databaseFile ) };
			return result;
		}

		public static FileInfo GetLog( FileInfo database )
		{
			var result = new FileInfo( Path.Combine( database.DirectoryName, string.Concat( Path.GetFileNameWithoutExtension( database.Name ), "_log.ldf" ) ) );
			return result;
		}
	}
}