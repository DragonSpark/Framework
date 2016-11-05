using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public class Path : SingletonScope<PathBase>, IPath
	{
		public static Path Default { get; } = new Path();
		Path() : base( DefaultImplementation.Implementation.Get ) {}

		[UsedImplicitly]
		public Path( PathBase source ) : base( source ) {}

		public string ChangeExtension( string path, string extension ) => Get().ChangeExtension( path, extension );
		public string Combine( params string[] paths ) => Get().Combine( paths );
		public string Combine( string path1, string path2 ) => Get().Combine( path1, path2 );
		public string Combine( string path1, string path2, string path3 ) => Get().Combine( path1, path2, path3 );
		public string Combine( string path1, string path2, string path3, string path4 ) => Get().Combine( path1, path2, path3, path4 );
		public string GetDirectoryName( string path ) => Get().GetDirectoryName( path );
		public string GetExtension( string path ) => Get().GetExtension( path );
		public string GetFileName( string path ) => Get().GetFileName( path );
		public string GetFileNameWithoutExtension( string path ) => Get().GetFileNameWithoutExtension( path );
		public string GetFullPath( string path ) => Get().GetFullPath( path );
		public char[] GetInvalidFileNameChars() => Get().GetInvalidFileNameChars();
		public char[] GetInvalidPathChars() => Get().GetInvalidPathChars();
		public string GetPathRoot( string path ) => Get().GetPathRoot( path );
		public string GetRandomFileName() => Get().GetRandomFileName();
		public string GetTempFileName() => Get().GetTempFileName();
		public string GetTempPath() => Get().GetTempPath();
		public bool HasExtension( string path ) => Get().HasExtension( path );
		public bool IsPathRooted( string path ) => Get().IsPathRooted( path );
		public char AltDirectorySeparatorChar => Get().AltDirectorySeparatorChar;
		public char DirectorySeparatorChar => Get().DirectorySeparatorChar;

		[Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
		public char[] InvalidPathChars => Get().InvalidPathChars;
		public char PathSeparator => Get().PathSeparator;
		public char VolumeSeparatorChar => Get().VolumeSeparatorChar;

		public sealed class DefaultImplementation : SourceBase<PathBase>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() {}

			public override PathBase Get() => new PathWrapper();
		}
	}
}