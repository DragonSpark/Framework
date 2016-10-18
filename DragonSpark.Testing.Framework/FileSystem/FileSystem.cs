using DragonSpark.Properties;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class FileSystem : IFileSystem, IFileSystemAccessor
	{
		readonly static string CurrentDirectory = System.IO.Path.GetTempPath();

		readonly IDictionary<string, IFileSystemElement> elements = new Dictionary<string, IFileSystemElement>( StringComparer.OrdinalIgnoreCase );

		public FileSystem() : this( Items<KeyValuePair<string, FileElement>>.Default ) {}

		public FileSystem( IEnumerable<KeyValuePair<string, FileElement>> files ) : this( files, CurrentDirectory ) {}

		public FileSystem( IEnumerable<KeyValuePair<string, FileElement>> files, string currentDirectory )
		{
			PathVerifier = new PathVerifier( this );
			Path = new MockPath( this );
			File = new MockFile( this );
			Directory = new MockDirectory( this, File, currentDirectory );
			FileInfo = new MockFileInfoFactory( this );
			DirectoryInfo = new MockDirectoryInfoFactory( this );
			DriveInfo = new MockDriveInfoFactory( this );
			foreach ( var file in files )
			{
				AddFile( file.Key, file.Value );
			}
		}

		public FileBase File { get; }

		public DirectoryBase Directory { get; }

		public IFileInfoFactory FileInfo { get; }

		public PathBase Path { get; }

		public IDirectoryInfoFactory DirectoryInfo { get; }

		public IDriveInfoFactory DriveInfo { get; }

		public PathVerifier PathVerifier { get; }

		public ImmutableArray<string> AllPaths => elements.Keys.ToImmutableArray();

		public ImmutableArray<string> AllFiles => elements.Where( f => f.Value is IFileElement ).Select( f => f.Key ).ToImmutableArray();

		public ImmutableArray<string> AllDirectories => elements.Where( f => f.Value is IDirectoryElement ).Select( f => f.Key ).ToImmutableArray();

		string FixPath( string path ) => Path.GetFullPath( path.Replace( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar ) );

		public IFileSystemElement GetElement( string path ) => GetFileWithoutFixingPath( FixPath( path ) );

		public void AddFile( string path, IFileElement file )
		{
			var key = FixPath( path );
			if ( FileExists( key ) && ( elements[key].Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly | ( elements[key].Attributes & FileAttributes.Hidden ) == FileAttributes.Hidden )
			{
				throw new UnauthorizedAccessException( string.Format( CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, path ) );
			}
			var name = Path.GetDirectoryName( key );
			if ( !Directory.Exists( name ) )
			{
				AddDirectory( name );
			}
			elements[key] = file;
		}

		public void AddDirectory( string path )
		{
			var index = FixPath( path );
			var str = MockUnixSupport.Separator();
			if ( FileExists( path ) && ( elements[index].Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly )
			{
				throw new UnauthorizedAccessException( string.Format( CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, path ) );
			}
			var local4 = 0;
			if ( path.StartsWith( @"\\", StringComparison.OrdinalIgnoreCase ) || path.StartsWith( "//", StringComparison.OrdinalIgnoreCase ) )
			{
				local4 = path.IndexOf( str, 2, StringComparison.OrdinalIgnoreCase );
				if ( local4 < 0 )
				{
					throw new ArgumentException( Resources.SERVER_PATH, "path" );
				}
			}
			while ( ( local4 = path.IndexOf( str, local4 + 1, StringComparison.OrdinalIgnoreCase ) ) > -1 )
			{
				var local10 = path.Substring( 0, local4 + 1 );
				if ( !Directory.Exists( local10 ) )
				{
					elements[local10] = new DirectoryElement();
				}
			}
			elements[path.EndsWith( str, StringComparison.OrdinalIgnoreCase ) ? path : path + str] = new DirectoryElement();
		}

		public void RemoveFile( string path ) => elements.Remove( FixPath( path ) );

		public bool FileExists( string path ) => !string.IsNullOrEmpty( path ) && elements.ContainsKey( FixPath( path ) );

		IFileSystemElement GetFileWithoutFixingPath( string path )
		{
			IFileSystemElement result;
			elements.TryGetValue( path, out result );
			return result;
		}
	}
}