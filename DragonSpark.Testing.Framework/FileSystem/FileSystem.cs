using DragonSpark.Properties;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IFileSystem : System.IO.Abstractions.IFileSystem, IFileInfoFactory, IDirectoryInfoFactory, IDriveInfoFactory
	{
		/// <summary>
		/// Gets a file.
		/// </summary>
		/// <param name="path">The path of the file to get.</param>
		/// <returns>The file. <see langword="null"/> if the file does not exist.</returns>
		IFileSystemElement GetElement(string path);

		void AddFile(string path, IFileElement file);
		void AddDirectory(string path);

		/// <summary>
		/// Removes the file.
		/// </summary>
		/// <param name="path">The file to remove.</param>
		/// <remarks>
		/// The file must not exist.
		/// </remarks>
		void RemoveFile(string path);

		/// <summary>
		/// Determines whether the file exists.
		/// </summary>
		/// <param name="path">The file to check. </param>
		/// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</returns>
		bool FileExists(string path);

		/// <summary>
		/// Gets all unique paths of all files and directories.
		/// </summary>
		ImmutableArray<string> AllPaths { get; }

		/// <summary>
		/// Gets the paths of all files.
		/// </summary>
		ImmutableArray<string> AllFiles { get; }

		/// <summary>
		/// Gets the paths of all directories.
		/// </summary>
		ImmutableArray<string> AllDirectories { get; }
	}

	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class FileSystem : IFileSystem
	{
		readonly static string CurrentDirectory = System.IO.Path.GetTempPath();

		readonly IDictionary<string, IFileSystemElement> elements = new Dictionary<string, IFileSystemElement>( StringComparer.OrdinalIgnoreCase );

		public FileSystem() : this( CurrentDirectory ) {}

		[UsedImplicitly]
		public FileSystem( string currentDirectory )
		{
			var mockPath = new MockPath( this );
			Path = mockPath;
			Directory = new MockDirectory( this, currentDirectory );
			File = new MockFile( this, mockPath );
		}

		public FileBase File { get; }

		public DirectoryBase Directory { get; }

		public PathBase Path { get; }

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

		public bool FileExists( string path ) => elements.ContainsKey( FixPath( path ) );

		IFileSystemElement GetFileWithoutFixingPath( string path )
		{
			IFileSystemElement result;
			elements.TryGetValue( path, out result );
			return result;
		}

		IDirectoryInfoFactory System.IO.Abstractions.IFileSystem.DirectoryInfo => this;
		public DirectoryInfoBase FromDirectoryName( string directoryName ) => new MockDirectoryInfo( this, directoryName );

		IFileInfoFactory System.IO.Abstractions.IFileSystem.FileInfo => this;
		public FileInfoBase FromFileName( string fileName ) => new MockFileInfo( this, fileName );

		IDriveInfoFactory System.IO.Abstractions.IFileSystem.DriveInfo => this;
		public DriveInfoBase[] GetDrives()
		{
			var driveLetters = new HashSet<string>(DriveEqualityComparer.Default);
			foreach (var path in AllPaths)
			{
				var pathRoot = Path.GetPathRoot(path);
				driveLetters.Add(pathRoot);
			}

			var result = new List<DriveInfoBase>();
			foreach (var driveLetter in driveLetters)
			{
				try
				{
					var mockDriveInfo = new MockDriveInfo(this, driveLetter);
					result.Add(mockDriveInfo);
				}
				catch (ArgumentException) {} // invalid drives should be ignored
			}

			return result.ToArray();
		}

		sealed class DriveEqualityComparer : IEqualityComparer<string>
		{
			public static DriveEqualityComparer Default { get; } = new DriveEqualityComparer();
			DriveEqualityComparer() {}

			public bool Equals(string x, string y) => ReferenceEquals( x, y ) || !ReferenceEquals( x, null ) && ( !ReferenceEquals( y, null ) && ( x[1] == ':' && y[1] == ':' && char.ToUpperInvariant( x[0] ) == char.ToUpperInvariant( y[0] ) ) );

			public int GetHashCode(string obj) => obj.ToUpperInvariant().GetHashCode();
		}
	}
}