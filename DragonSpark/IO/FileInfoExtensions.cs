using System;
using System.IO;

namespace DragonSpark.Io
{
	public static class FileInfoExtensions
	{
		public static FileInfo Rename( this FileInfo target, string name, string extension = null )
		{
			if ( string.Compare( name, target.Name, StringComparison.InvariantCultureIgnoreCase ) != 0 )
			{
				var path = Path.Combine( target.DirectoryName, Path.ChangeExtension( name, extension ?? Path.GetExtension( name ) ) );
				if ( File.Exists( path ) )
				{
					File.Delete( path );
				}
				target.MoveTo( path );
			}
			return target;
		}
	}
}