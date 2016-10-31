using DragonSpark.Sources;
using JetBrains.Annotations;
using System;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public class Path : IPath
	{
		public static IScope<IPath> Current { get; } = new Scope<IPath>( Sources.Factory.GlobalCache( () => new Path() ) );
		Path() : this( DefaultImplementation.Implementation.Get() ) {}

		readonly PathBase source;

		[UsedImplicitly]
		public Path( PathBase source )
		{
			this.source = source;
		}

		public string ChangeExtension( string path, string extension ) => source.ChangeExtension( path, extension );

		public string Combine( params string[] paths ) => source.Combine( paths );

		public string Combine( string path1, string path2 ) => source.Combine( path1, path2 );

		public string Combine( string path1, string path2, string path3 ) => source.Combine( path1, path2, path3 );

		public string Combine( string path1, string path2, string path3, string path4 ) => source.Combine( path1, path2, path3, path4 );

		public string GetDirectoryName( string path ) => source.GetDirectoryName( path );

		public string GetExtension( string path ) => source.GetExtension( path );

		public string GetFileName( string path ) => source.GetFileName( path );

		public string GetFileNameWithoutExtension( string path ) => source.GetFileNameWithoutExtension( path );

		public string GetFullPath( string path ) => source.GetFullPath( path );

		public char[] GetInvalidFileNameChars() => source.GetInvalidFileNameChars();

		public char[] GetInvalidPathChars() => source.GetInvalidPathChars();

		public string GetPathRoot( string path ) => source.GetPathRoot( path );

		public string GetRandomFileName() => source.GetRandomFileName();

		public string GetTempFileName() => source.GetTempFileName();

		public string GetTempPath() => source.GetTempPath();

		public bool HasExtension( string path ) => source.HasExtension( path );

		public bool IsPathRooted( string path ) => source.IsPathRooted( path );

		public char AltDirectorySeparatorChar => source.AltDirectorySeparatorChar;

		public char DirectorySeparatorChar => source.DirectorySeparatorChar;

		[Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
		public char[] InvalidPathChars => source.InvalidPathChars;

		public char PathSeparator => source.PathSeparator;

		public char VolumeSeparatorChar => source.VolumeSeparatorChar;

		public sealed class DefaultImplementation : Scope<PathBase>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( Sources.Factory.GlobalCache( () => new PathWrapper() ) ) {}
		}
	}
}