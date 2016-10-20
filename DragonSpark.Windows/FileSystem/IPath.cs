using System;

namespace DragonSpark.Windows.FileSystem
{
	public interface IPath
	{
		char AltDirectorySeparatorChar { get; }

		char DirectorySeparatorChar { get; }

		[Obsolete( "Please use GetInvalidPathChars or GetInvalidFileNameChars instead." )]
		char[] InvalidPathChars { get; }

		char PathSeparator { get; }

		char VolumeSeparatorChar { get; }

		string ChangeExtension( string path, string extension );

		string Combine( params string[] paths );

		string Combine( string path1, string path2 );

		string Combine( string path1, string path2, string path3 );

		string Combine( string path1, string path2, string path3, string path4 );

		string GetDirectoryName( string path );

		string GetExtension( string path );

		string GetFileName( string path );

		string GetFileNameWithoutExtension( string path );

		string GetFullPath( string path );

		char[] GetInvalidFileNameChars();

		char[] GetInvalidPathChars();

		string GetPathRoot( string path );

		string GetRandomFileName();

		string GetTempFileName();

		string GetTempPath();

		bool HasExtension( string path );

		bool IsPathRooted( string path );
	}
}