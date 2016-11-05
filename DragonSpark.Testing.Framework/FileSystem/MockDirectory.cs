using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockDirectory : DirectoryBase
	{
		readonly static string AllDirectoriesPattern = Defaults.IsUnix ? @"([^<>:""/|?*]*/)*" : @"([^<>:""/\\|?*]*\\)*";
		readonly static string FileNamePattern = Defaults.IsUnix ? @"[^/]*?/?" : @"[^\\]*?\\?";

		readonly IFileSystemRepository repository;
		readonly IPath path;
		readonly IDirectorySource directory;
		
		public MockDirectory() : this( FileSystemRepository.Default, Windows.FileSystem.Path.Default, DirectorySource.Default ) {}

		[UsedImplicitly]
		public MockDirectory(IFileSystemRepository repository, IPath path, IDirectorySource directory )
		{
			this.repository = repository;
			this.path = path;
			this.directory = directory;
		}

		public override DirectoryInfoBase CreateDirectory(string pathName) => CreateDirectory(pathName, null);

		public override DirectoryInfoBase CreateDirectory(string pathName, DirectorySecurity directorySecurity)
		{
			var current = repository.Get( pathName );
			if ( current is IFileElement )
			{
				var message = string.Format(CultureInfo.InvariantCulture, @"Cannot create ""{0}"" because a file or directory with the same name already exists.", pathName);
				var ex = new IOException(message);
				ex.Data.Add(nameof(pathName), pathName);
				throw ex;
			}

			var result = repository.FromDirectoryName( pathName );
			if (current == null)
			{
				repository.Set( pathName, new DirectoryElement() );
			}
			return result;
		}

		public override void Delete(string pathName) => Delete(pathName, false);

		public override void Delete(string pathName, bool recursive)
		{
			var name = path.EnsureTrailingSlash( path.Normalize( pathName ) );
			var affectedPaths = repository
				.AllPaths
				.Where(p => p.StartsWith(name, StringComparison.OrdinalIgnoreCase))
				.ToList();

			if (!affectedPaths.Any())
				throw new DirectoryNotFoundException( $"{name} does not exist or could not be found." );

			if (!recursive && affectedPaths.Count > 1)
				throw new IOException( $"The directory specified by {name} is read-only, or recursive is false and {name} is not an empty directory." );

			foreach (var affectedPath in affectedPaths)
				repository.Remove(affectedPath);
		}

		public override bool Exists(string pathName) => repository.Get( pathName ) is IDirectoryElement;

		public override DirectorySecurity GetAccessControl(string pathName)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override DirectorySecurity GetAccessControl(string pathName, AccessControlSections includeSections)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override string GetCurrentDirectory() => directory.Get();
		public override void SetCurrentDirectory(string pathName) => directory.Assign( pathName );

		public override string GetDirectoryRoot(string pathName) => path.GetPathRoot(pathName);

		public override string[] GetDirectories( string pathName ) => EnumerateDirectories( pathName ).ToArray();
		public override string[] GetDirectories( string pathName, string searchPattern ) => EnumerateDirectories( pathName, searchPattern ).ToArray();
		public override string[] GetDirectories( string pathName, string searchPattern, SearchOption searchOption ) => EnumerateDirectories( pathName, searchPattern, searchOption ).ToArray();
		public override IEnumerable<string> EnumerateDirectories( string pathName ) => EnumerateDirectories( pathName, Defaults.AllPattern );
		public override IEnumerable<string> EnumerateDirectories( string pathName, string searchPattern ) => EnumerateDirectories( pathName, searchPattern, SearchOption.TopDirectoryOnly );
		public override IEnumerable<string> EnumerateDirectories( string pathName, string searchPattern, SearchOption searchOption ) =>
			Search( pathName, repository.AllDirectories, searchPattern, searchOption );

		public override string[] GetFiles( string pathName ) => EnumerateFiles( pathName ).ToArray();
		public override string[] GetFiles( string pathName, string searchPattern ) => EnumerateFiles( pathName, searchPattern ).ToArray();
		public override string[] GetFiles( string pathName, string searchPattern, SearchOption searchOption ) => EnumerateFiles( pathName, searchPattern, searchOption ).ToArray();
		public override IEnumerable<string> EnumerateFiles( string pathName ) => EnumerateFiles( pathName, Defaults.AllPattern );
		public override IEnumerable<string> EnumerateFiles( string pathName, string searchPattern ) => EnumerateFiles( pathName, searchPattern, SearchOption.TopDirectoryOnly );
		public override IEnumerable<string> EnumerateFiles( string pathName, string searchPattern, SearchOption searchOption ) => 
			Search( pathName, repository.AllFiles, searchPattern, searchOption );

		public override string[] GetFileSystemEntries( string pathName ) => EnumerateFileSystemEntries( pathName ).ToArray();
		public override string[] GetFileSystemEntries( string pathName, string searchPattern ) => EnumerateFileSystemEntries( pathName, searchPattern ).ToArray();
		public override IEnumerable<string> EnumerateFileSystemEntries( string pathName ) => EnumerateFileSystemEntries( pathName, Defaults.AllPattern );
		public override IEnumerable<string> EnumerateFileSystemEntries(string pathName, string searchPattern) => EnumerateFileSystemEntries( pathName, Defaults.AllPattern, SearchOption.TopDirectoryOnly );
		public override IEnumerable<string> EnumerateFileSystemEntries( string pathName, string searchPattern, SearchOption searchOption ) => 
			Search( pathName, repository.AllPaths, searchPattern, SearchOption.TopDirectoryOnly );

		IEnumerable<string> Search( string pathName, ImmutableArray<string> names, string searchPattern, SearchOption searchOption )
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			if ( !Exists( pathName ) )
			{
				throw new DirectoryNotFoundException( string.Format( CultureInfo.InvariantCulture, Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, pathName ) );
			}

			CheckSearchPattern(searchPattern);

			var normalized = path.Normalize( pathName );
			var escaped = Regex.Escape( normalized );

			var separator = Regex.Escape( path.DirectorySeparatorChar.ToString() );
			var option = searchOption == SearchOption.AllDirectories ? AllDirectoriesPattern : string.Empty;
			var patterns = new List<string>();
			switch ( searchPattern )
			{
				case Defaults.AllPattern:
					patterns.Add( FileNamePattern );
					break;
				default:
					var slashes = string.Concat( "/", Defaults.IsUnix ? string.Empty : separator );
					var pattern = $@"[^<>:""{slashes}|?*]";
					var fileNamePattern = Regex.Escape( searchPattern )
											   .Replace( @"\*", $@"{pattern}*?" )
											   .Replace( @"\?", $@"{pattern}?" );
					patterns.Add( fileNamePattern );

					var extension = path.GetExtension( searchPattern );
					var hasExtensionLengthOfThree = extension.Length == 4 && !extension.Contains( Defaults.AllPattern ) && !extension.Contains( "?" );
					if ( hasExtensionLengthOfThree )
					{
						patterns.Add( $"{fileNamePattern}[^.]" );
					}
					break;
			}
			var checks = patterns.Select( s => $@"(?i:^{escaped}{separator}{option}{s}(?:{separator}?)$)" ).ToArray();
			var result = names
				.Where( file => checks.Any( pattern => Regex.IsMatch( file, pattern ) ) )
				.Where( p => string.Compare( p, normalized, StringComparison.OrdinalIgnoreCase ) != 0 );
			return result;
		}

		public override string[] GetLogicalDrives() => repository
			.AllDirectories
			.Select(d => repository.FromDirectoryName( d ).Root.FullName.ToLowerInvariant() )
			.Distinct()
			.ToArray();

		public override DirectoryInfoBase GetParent(string pathName)
		{
			if ( !path.IsValidPath( pathName ) )
			{
				throw new ArgumentException(Resources.INVALID_CHARACTERS, nameof(pathName) );
			}

			var collection = path.GetFullPath( pathName ).ToStringArray( path.DirectorySeparatorChar ).ToArray();
			if ( collection.Length > 1 )
			{
				var parts = new Stack<string>( collection );
				parts.Pop();
				var name = parts.Count == 1 ? path.GetPathRoot( pathName ).NullIfEmpty() ?? directory.PathRoot : string.Join( path.DirectorySeparatorChar.ToString(), parts.Reverse() );
				var result = repository.FromDirectoryName( name );
				return result;
			}
			return null;
		}

		public override void Move(string sourceDirName, string destDirName) 
		{
			var fullSourcePath = path.Normalize(sourceDirName);
			var fullDestPath = path.Normalize(destDirName);

			if (string.Equals(fullSourcePath, fullDestPath, StringComparison.OrdinalIgnoreCase))
			{
				throw new IOException("Source and destination path must be different.");
			}

			var sourceRoot = path.GetPathRoot(fullSourcePath);
			var destinationRoot = path.GetPathRoot(fullDestPath);
			if (!string.Equals(sourceRoot, destinationRoot, StringComparison.OrdinalIgnoreCase))
			{
				throw new IOException("Source and destination path must have identical roots. Move will not work across volumes.");
			}

			//Make sure that the destination exists
			CreateDirectory(fullDestPath);

			//Recursively move all the subdirectories from the source into the destination directory
			var subdirectories = GetDirectories(fullSourcePath);
			foreach (var subdirectory in subdirectories)
			{
				var newSubdirPath = subdirectory.Replace(fullSourcePath, fullDestPath, StringComparison.OrdinalIgnoreCase);
				Move(subdirectory, newSubdirPath);
			}

			//Move the files in destination directory
			var files = GetFiles(fullSourcePath);
			foreach (var f in files)
			{
				var newFilePath = f.Replace(fullSourcePath, fullDestPath, StringComparison.OrdinalIgnoreCase);
				repository.FromFileName(f).MoveTo(newFilePath);
			}

			//Delete the source directory
			Delete(fullSourcePath);
		}

		public override void SetAccessControl(string pathName, DirectorySecurity directorySecurity)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override DateTime GetCreationTime(string pathName) => repository.Get( pathName ).CreationTime.DateTime;
		public override void SetCreationTime(string pathName, DateTime creationTime) => repository.GetDirectory( pathName ).CreationTime = creationTime;
		public override DateTime GetCreationTimeUtc(string pathName) => repository.Get( pathName ).CreationTime.UtcDateTime;
		public override void SetCreationTimeUtc( string pathName, DateTime creationTimeUtc ) => repository.GetDirectory( pathName ).CreationTime = creationTimeUtc;

		public override DateTime GetLastAccessTime(string pathName) => repository.Get( pathName ).LastAccessTime.DateTime;
		public override void SetLastAccessTime(string pathName, DateTime lastAccessTime) => repository.GetDirectory( pathName ).LastAccessTime = lastAccessTime;
		public override DateTime GetLastAccessTimeUtc(string pathName) => repository.Get( pathName ).LastAccessTime.UtcDateTime;
		public override void SetLastAccessTimeUtc(string pathName, DateTime lastAccessTimeUtc) => repository.GetDirectory( pathName ).LastAccessTime = lastAccessTimeUtc;

		public override DateTime GetLastWriteTime(string pathName) => repository.Get( pathName ).LastWriteTime.DateTime;
		public override void SetLastWriteTime(string pathName, DateTime lastWriteTime) => repository.GetDirectory( pathName ).LastWriteTime = lastWriteTime;
		public override DateTime GetLastWriteTimeUtc(string pathName) => repository.Get( pathName ).LastWriteTime.UtcDateTime;
		public override void SetLastWriteTimeUtc(string pathName, DateTime lastWriteTimeUtc) => repository.GetDirectory( pathName ).LastWriteTime = lastWriteTimeUtc;

		void CheckSearchPattern(string searchPattern)
		{
			Func<ArgumentException> createException = () => new ArgumentException($@"Search pattern cannot contain ""{Windows.FileSystem.Defaults.ParentPath}"" to move up directories and can be contained only internally in file/directory names, as in ""a..b"".", searchPattern);

			if (searchPattern.EndsWith(Windows.FileSystem.Defaults.ParentPath, StringComparison.OrdinalIgnoreCase))
			{
				throw createException();
			}

			int position;
			if ((position = searchPattern.IndexOf(Windows.FileSystem.Defaults.ParentPath, StringComparison.OrdinalIgnoreCase)) >= 0)
			{
				var characterAfterTwoDots = searchPattern[position + 2];
				if (characterAfterTwoDots == path.DirectorySeparatorChar || characterAfterTwoDots == path.AltDirectorySeparatorChar)
				{
					throw createException();
				}
			}

			if ( !path.IsValidPath( searchPattern ) )
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION, nameof( searchPattern ));
			}
		}
	}
}