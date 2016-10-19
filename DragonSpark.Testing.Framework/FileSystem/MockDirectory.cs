using DragonSpark.Properties;
using DragonSpark.Windows.FileSystem;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using Path = System.IO.Path;
using XFS = DragonSpark.Testing.Framework.FileSystem.MockUnixSupport;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockDirectory : DirectoryBase
	{
		readonly IFileSystemRepository repository;
		readonly IPath path;
		readonly IFile file;
		
		string currentDirectory;

		public MockDirectory() : this( FileSystemRepository.Current.Get(), Windows.FileSystem.Path.Current.Get(), Windows.FileSystem.File.Current.Get(), Defaults.DirectoryName ) {}

		public MockDirectory(IFileSystemRepository repository, IPath path, IFile file, string currentDirectory)
		{
			this.currentDirectory = currentDirectory;
			this.repository = repository;
			this.path = path;
			this.file = file;
		}

		public override DirectoryInfoBase CreateDirectory(string pathName) => CreateDirectory(pathName, null);

		public override DirectoryInfoBase CreateDirectory(string pathName, DirectorySecurity directorySecurity)
		{
			if (pathName.Length == 0)
			{
				throw new ArgumentException(Properties.Resources.PATH_CANNOT_BE_THE_EMPTY_STRING_OR_ALL_WHITESPACE, nameof( pathName ));
			}

			if (repository.FileExists(pathName))
			{
				var message = string.Format(CultureInfo.InvariantCulture, @"Cannot create ""{0}"" because a file or directory with the same name already exists.", pathName);
				var ex = new IOException(message);
				ex.Data.Add("Path", pathName);
				throw ex;
			}

			pathName = EnsurePathEndsWithDirectorySeparator(path.GetFullPath(pathName));

			if (!Exists(pathName))
			{
				repository.AddDirectory(pathName);
			}

			var created = repository.FromDirectoryName( pathName );
			return created;
		}

		public override void Delete(string pathName) => Delete(pathName, false);

		public override void Delete(string pathName, bool recursive)
		{
			pathName = EnsurePathEndsWithDirectorySeparator(path.GetFullPath(pathName));
			var affectedPaths = repository
				.AllPaths
				.Where(p => p.StartsWith(pathName, StringComparison.OrdinalIgnoreCase))
				.ToList();

			if (!affectedPaths.Any())
				throw new DirectoryNotFoundException(pathName + " does not exist or could not be found.");

			if (!recursive &&
				affectedPaths.Count > 1)
				throw new IOException("The directory specified by " + pathName + " is read-only, or recursive is false and " + pathName + " is not an empty directory.");

			foreach (var affectedPath in affectedPaths)
				repository.RemoveFile(affectedPath);
		}

		public override bool Exists(string pathName)
		{
			try
			{
				var name = path.GetFullPath(EnsurePathEndsWithDirectorySeparator(pathName));
				return repository.AllDirectories.Any(p => p.Equals(name, StringComparison.OrdinalIgnoreCase));
			}
			catch (Exception)
			{
				return false;
			}
		}

		public override DirectorySecurity GetAccessControl(string pathName)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override DirectorySecurity GetAccessControl(string pathName, AccessControlSections includeSections)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override DateTime GetCreationTime(string pathName) => file.GetCreationTime(pathName);

		public override DateTime GetCreationTimeUtc(string pathName) => file.GetCreationTimeUtc(pathName);

		public override string GetCurrentDirectory() => currentDirectory;

		public override string[] GetDirectories(string pathName) => GetDirectories(pathName, "*");

		public override string[] GetDirectories(string pathName, string searchPattern) => GetDirectories(pathName, searchPattern, SearchOption.TopDirectoryOnly);

		public override string[] GetDirectories(string pathName, string searchPattern, SearchOption searchOption) => EnumerateDirectories(pathName, searchPattern, searchOption).ToArray();

		public override string GetDirectoryRoot(string pathName) => Path.GetPathRoot(pathName);

		public override string[] GetFiles(string pathName) => GetFiles(pathName, "*");

		public override string[] GetFiles(string pathName, string searchPattern) => GetFiles(pathName, searchPattern, SearchOption.TopDirectoryOnly);

		public override string[] GetFiles(string pathName, string searchPattern, SearchOption searchOption)
		{
			if (!Exists(pathName))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, pathName));
			}

			return GetFilesInternal(repository.AllFiles, pathName, searchPattern, searchOption);
		}

		string[] GetFilesInternal(ImmutableArray<string> files, string pathName, string searchPattern, SearchOption searchOption)
		{
			CheckSearchPattern(searchPattern);
			pathName = EnsurePathEndsWithDirectorySeparator(pathName);
			pathName = path.GetFullPath(pathName);

			bool isUnix = XFS.IsUnixPlatform();

			string allDirectoriesPattern = isUnix
				? @"([^<>:""/|?*]*/)*"
				: @"([^<>:""/\\|?*]*\\)*";

			string fileNamePattern;
			string pathPatternSpecial = null;
			if (searchPattern == "*")
			{
				fileNamePattern = isUnix ? @"[^/]*?/?" : @"[^\\]*?\\?";
			}
			else
			{
				fileNamePattern = Regex.Escape(searchPattern)
					.Replace(@"\*", isUnix ? @"[^<>:""/|?*]*?" : @"[^<>:""/\\|?*]*?")
					.Replace(@"\?", isUnix ? @"[^<>:""/|?*]?" : @"[^<>:""/\\|?*]?");

				var extension = Path.GetExtension(searchPattern);
				bool hasExtensionLengthOfThree = extension.Length == 4 && !extension.Contains("*") && !extension.Contains("?");
				if (hasExtensionLengthOfThree)
				{
					var fileNamePatternSpecial = string.Format(CultureInfo.InvariantCulture, "{0}[^.]", fileNamePattern);
					pathPatternSpecial = string.Format(
						CultureInfo.InvariantCulture,
						isUnix ? @"(?i:^{0}{1}{2}(?:/?)$)" : @"(?i:^{0}{1}{2}(?:\\?)$)",
						Regex.Escape(pathName),
						searchOption == SearchOption.AllDirectories ? allDirectoriesPattern : string.Empty,
						fileNamePatternSpecial);
				}
			}

			var pathPattern = string.Format(
				CultureInfo.InvariantCulture,
				isUnix ? @"(?i:^{0}{1}{2}(?:/?)$)" : @"(?i:^{0}{1}{2}(?:\\?)$)",
				Regex.Escape(pathName),
				searchOption == SearchOption.AllDirectories ? allDirectoriesPattern : string.Empty,
				fileNamePattern);


			return files
				.Where(p =>
					{
						if (Regex.IsMatch(p, pathPattern))
						{
							return true;
						}

						if (pathPatternSpecial != null && Regex.IsMatch(p, pathPatternSpecial))
						{
							return true;
						}

						return false;
					})
				.ToArray();
		}

		public override string[] GetFileSystemEntries(string pathName) => GetFileSystemEntries(pathName, "*");

		public override string[] GetFileSystemEntries(string pathName, string searchPattern) => GetDirectories(pathName, searchPattern).Union( GetFiles(pathName, searchPattern) ).ToArray();

		public override DateTime GetLastAccessTime(string pathName) => file.GetLastAccessTime(pathName);

		public override DateTime GetLastAccessTimeUtc(string pathName) => file.GetLastAccessTimeUtc(pathName);

		public override DateTime GetLastWriteTime(string pathName) => file.GetLastWriteTime(pathName);

		public override DateTime GetLastWriteTimeUtc(string pathName) => file.GetLastWriteTimeUtc(pathName);

		public override string[] GetLogicalDrives() => repository
			.AllDirectories
			.Select(d => repository.FromDirectoryName( d ).Root.FullName.ToLowerInvariant() )
			.Distinct()
			.ToArray();

		public override DirectoryInfoBase GetParent(string pathName)
		{
			if (MockPath.HasIllegalCharacters(pathName, false))
			{
				throw new ArgumentException(Resources.INVALID_CHARACTERS, nameof(pathName));
			}

			var absolutePath = path.GetFullPath(pathName);
			var sepAsString = path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

			var lastIndex = 0;
			if (absolutePath != sepAsString)
			{
				var startIndex = absolutePath.EndsWith(sepAsString, StringComparison.OrdinalIgnoreCase) ? absolutePath.Length - 1 : absolutePath.Length;
				lastIndex = absolutePath.LastIndexOf(path.DirectorySeparatorChar, startIndex - 1);
				if (lastIndex < 0)
				{
					return null;
				}
			}

			var parentPath = absolutePath.Substring(0, lastIndex);
			var parent = !string.IsNullOrEmpty(parentPath) ? repository.FromDirectoryName(parentPath) : null;
			return parent;
		}

		public override void Move(string sourceDirName, string destDirName)
		{
			var fullSourcePath = EnsurePathEndsWithDirectorySeparator(path.GetFullPath(sourceDirName));
			var fullDestPath = EnsurePathEndsWithDirectorySeparator(path.GetFullPath(destDirName));

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

		public override void SetCreationTime(string pathName, DateTime creationTime) => file.SetCreationTime(pathName, creationTime);

		public override void SetCreationTimeUtc(string pathName, DateTime creationTimeUtc) => file.SetCreationTimeUtc(pathName, creationTimeUtc);

		public override void SetCurrentDirectory(string pathName) => currentDirectory = pathName;

		public override void SetLastAccessTime(string pathName, DateTime lastAccessTime) => file.SetLastAccessTime(pathName, lastAccessTime);

		public override void SetLastAccessTimeUtc(string pathName, DateTime lastAccessTimeUtc) => file.SetLastAccessTimeUtc(pathName, lastAccessTimeUtc);

		public override void SetLastWriteTime(string pathName, DateTime lastWriteTime) => file.SetLastWriteTime(pathName, lastWriteTime);

		public override void SetLastWriteTimeUtc(string pathName, DateTime lastWriteTimeUtc) => file.SetLastWriteTimeUtc(pathName, lastWriteTimeUtc);

		public override IEnumerable<string> EnumerateDirectories(string pathName)
		{
			repository.IsLegalAbsoluteOrRelative(pathName, "path");

			return EnumerateDirectories(pathName, "*");
		}

		public override IEnumerable<string> EnumerateDirectories(string pathName, string searchPattern)
		{
			repository.IsLegalAbsoluteOrRelative(pathName, "path");

			return EnumerateDirectories(pathName, searchPattern, SearchOption.TopDirectoryOnly);
		}

		public override IEnumerable<string> EnumerateDirectories(string pathName, string searchPattern, SearchOption searchOption)
		{
			repository.IsLegalAbsoluteOrRelative(pathName, "path");

			pathName = EnsurePathEndsWithDirectorySeparator(pathName);

			if (!Exists(pathName))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, pathName));
			}

			var dirs = GetFilesInternal(repository.AllDirectories, pathName, searchPattern, searchOption);
			return dirs.Where(p => string.Compare(p, pathName, StringComparison.OrdinalIgnoreCase) != 0);
		}

		public override IEnumerable<string> EnumerateFiles(string pathName) => GetFiles(pathName);

		public override IEnumerable<string> EnumerateFiles(string pathName, string searchPattern) => GetFiles(pathName, searchPattern);

		public override IEnumerable<string> EnumerateFiles(string pathName, string searchPattern, SearchOption searchOption) => GetFiles(pathName, searchPattern, searchOption);

		public override IEnumerable<string> EnumerateFileSystemEntries(string pathName) => new List<string>(GetFiles(pathName).Concat( GetDirectories(pathName) ));

		public override IEnumerable<string> EnumerateFileSystemEntries(string pathName, string searchPattern) => new List<string>(GetFiles(pathName, searchPattern).Concat( GetDirectories(pathName) ));

		public override IEnumerable<string> EnumerateFileSystemEntries(string pathName, string searchPattern, SearchOption searchOption) => new List<string>(GetFiles(pathName, searchPattern, searchOption).Concat( GetDirectories(pathName, searchPattern, searchOption) ));

		static string EnsurePathEndsWithDirectorySeparator(string path)
		{
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
				path += Path.DirectorySeparatorChar;
			return path;
		}

		static void CheckSearchPattern(string searchPattern)
		{
			const string twoDots = "..";
			Func<ArgumentException> createException = () => new ArgumentException(@"Search pattern cannot contain "".."" to move up directories and can be contained only internally in file/directory names, as in ""a..b"".", searchPattern);

			if (searchPattern.EndsWith(twoDots, StringComparison.OrdinalIgnoreCase))
			{
				throw createException();
			}

			int position;
			if ((position = searchPattern.IndexOf(twoDots, StringComparison.OrdinalIgnoreCase)) >= 0)
			{
				var characterAfterTwoDots = searchPattern[position + 2];
				if (characterAfterTwoDots == Path.DirectorySeparatorChar || characterAfterTwoDots == Path.AltDirectorySeparatorChar)
				{
					throw createException();
				}
			}

			var invalidPathChars = Path.GetInvalidPathChars();
			if (searchPattern.IndexOfAny(invalidPathChars) > -1)
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION, nameof( searchPattern ));
			}
		}
	}
}