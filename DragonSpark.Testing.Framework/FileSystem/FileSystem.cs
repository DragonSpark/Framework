using DragonSpark.Properties;
using DragonSpark.Sources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IFileSystemRepository : IFileInfoFactory, IDirectoryInfoFactory, IDriveInfoFactory
	{
		/// <summary>
		/// Gets a file.
		/// </summary>
		/// <param name="path">The path of the file to get.</param>
		/// <returns>The file. <see langword="null"/> if the file does not exist.</returns>
		IFileSystemElement GetElement(string path);

		void AddFile(IFileElement file);
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

		void IsLegalAbsoluteOrRelative( string pathToValidate, string paramName );
	}

	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class FileSystemRepository : IFileSystemRepository
	{
		readonly static char[] InvalidChars = Path.GetInvalidFileNameChars();

		public static IScope<IFileSystemRepository> Current { get; } = new Scope<IFileSystemRepository>( Factory.GlobalCache( () => new FileSystemRepository() ) );
		FileSystemRepository() {}


		readonly IDictionary<string, IFileSystemElement> elements = new Dictionary<string, IFileSystemElement>( StringComparer.OrdinalIgnoreCase );

		public ImmutableArray<string> AllPaths => elements.Keys.ToImmutableArray();

		public ImmutableArray<string> AllFiles => elements.Where( f => f.Value is IFileElement ).Select( f => f.Key ).ToImmutableArray();

		public ImmutableArray<string> AllDirectories => elements.Where( f => f.Value is IDirectoryElement ).Select( f => f.Key ).ToImmutableArray();

		string FixPath( string path ) => Path.GetFullPath( path.Replace( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar ) );

		public IFileSystemElement GetElement( string path ) => GetFileWithoutFixingPath( FixPath( path ) );

		public void AddFile( IFileElement file )
		{
			var key = FixPath( file.Path );
			if ( FileExists( key ) && ( elements[key].Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly | ( elements[key].Attributes & FileAttributes.Hidden ) == FileAttributes.Hidden )
			{
				throw new UnauthorizedAccessException( string.Format( CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, file.Path ) );
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
					throw new ArgumentException( Resources.SERVER_PATH, nameof( path ) );
				}
			}
			while ( ( local4 = path.IndexOf( str, local4 + 1, StringComparison.OrdinalIgnoreCase ) ) > -1 )
			{
				var local10 = path.Substring( 0, local4 + 1 );
				if ( !Directory.Exists( local10 ) )
				{
					elements[local10] = new DirectoryElement( local10 );
				}
			}
			var key = path.EndsWith( str, StringComparison.OrdinalIgnoreCase ) ? path : path + str;
			elements[key] = new DirectoryElement( key );
		}

		public void RemoveFile( string path ) => elements.Remove( FixPath( path ) );

		public bool FileExists( string path ) => elements.ContainsKey( FixPath( path ) );

		IFileSystemElement GetFileWithoutFixingPath( string path )
		{
			IFileSystemElement result;
			elements.TryGetValue( path, out result );
			return result;
		}

		public DirectoryInfoBase FromDirectoryName( string directoryName ) => new MockDirectoryInfo( this, directoryName );

		public FileInfoBase FromFileName( string fileName ) => new MockFileInfo( this, fileName );

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
					result.Add( new MockDriveInfo( this, driveLetter ) );
				}
				catch (ArgumentException) {} // invalid drives should be ignored
			}

			return result.ToArray();
		}
		
		public void IsLegalAbsoluteOrRelative(string pathToValidate, string paramName)
		{
			if (pathToValidate.Trim() == string.Empty)
			{
				throw new ArgumentException(Properties.Resources.THE_PATH_IS_NOT_OF_A_LEGAL_FORM, paramName);
			}

			if (ExtractFileName(pathToValidate).IndexOfAny(InvalidChars) > -1)
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
			}

			var filePath = ExtractFilePath(pathToValidate);
			if (MockPath.HasIllegalCharacters(filePath, false))
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
			}
		}

		static string ExtractFileName(string fullFileName) => fullFileName.Split(Path.DirectorySeparatorChar).Last();

		static string ExtractFilePath(string fullFileName)
		{
			var extractFilePath = fullFileName.Split(Path.DirectorySeparatorChar);
			return string.Join(Path.DirectorySeparatorChar.ToString(), extractFilePath.Take(extractFilePath.Length - 1));
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