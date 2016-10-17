using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Legacy.Entity
{
	public static class EntityFiles
	{
		public static DirectoryInfoBase DefaultDataDirectory { get; } = new DirectoryInfo( @".\App_Data" );

		public static IEnumerable<FileInfoBase> WithLog( FileInfoBase databaseFile ) => new[] { databaseFile, GetLog( databaseFile ) };

		public static FileInfoBase GetLog( FileInfoBase database ) => new FileInfo( Path.Combine( database.DirectoryName ?? string.Empty, string.Concat( Path.GetFileNameWithoutExtension( database.Name ), "_log.ldf" ) ) );
	}
}