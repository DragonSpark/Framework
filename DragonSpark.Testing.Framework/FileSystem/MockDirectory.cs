using DragonSpark.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace DragonSpark.Testing.Framework.FileSystem
{
	using XFS = MockUnixSupport;

	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockDirectory : DirectoryBase
	{
		readonly IFileSystem fileSystem;
		readonly IPathingValidator validator;

		private string currentDirectory;

		public MockDirectory(IFileSystem fileSystem, string currentDirectory) : this( fileSystem, PathingValidator.Default, currentDirectory ) {}

		public MockDirectory(IFileSystem fileSystem, IPathingValidator validator, string currentDirectory)
		{
			this.currentDirectory = currentDirectory;
			this.fileSystem = fileSystem;
			this.validator = validator;
		}

		public override DirectoryInfoBase CreateDirectory(string path) => CreateDirectory(path, null);

		public override DirectoryInfoBase CreateDirectory(string path, DirectorySecurity directorySecurity)
		{
			if (path.Length == 0)
			{
				throw new ArgumentException(Properties.Resources.PATH_CANNOT_BE_THE_EMPTY_STRING_OR_ALL_WHITESPACE, "path");
			}

			if (fileSystem.FileExists(path))
			{
				var message = string.Format(CultureInfo.InvariantCulture, @"Cannot create ""{0}"" because a file or directory with the same name already exists.", path);
				var ex = new IOException(message);
				ex.Data.Add("Path", path);
				throw ex;
			}

			path = EnsurePathEndsWithDirectorySeparator(fileSystem.Path.GetFullPath(path));

			if (!Exists(path))
			{
				fileSystem.AddDirectory(path);
			}

			var created = new MockDirectoryInfo(fileSystem, path);
			return created;
		}

		public override void Delete(string path)
		{
			Delete(path, false);
		}

		public override void Delete(string path, bool recursive)
		{
			path = EnsurePathEndsWithDirectorySeparator(fileSystem.Path.GetFullPath(path));
			var affectedPaths = fileSystem
				.AllPaths
				.Where(p => p.StartsWith(path, StringComparison.OrdinalIgnoreCase))
				.ToList();

			if (!affectedPaths.Any())
				throw new DirectoryNotFoundException(path + " does not exist or could not be found.");

			if (!recursive &&
				affectedPaths.Count > 1)
				throw new IOException("The directory specified by " + path + " is read-only, or recursive is false and " + path + " is not an empty directory.");

			foreach (var affectedPath in affectedPaths)
				fileSystem.RemoveFile(affectedPath);
		}

		public override bool Exists(string path)
		{
			try
			{
				path = EnsurePathEndsWithDirectorySeparator(path);

				path = fileSystem.Path.GetFullPath(path);
				return fileSystem.AllDirectories.Any(p => p.Equals(path, StringComparison.OrdinalIgnoreCase));
			}
			catch (Exception)
			{
				return false;
			}
		}

		public override DirectorySecurity GetAccessControl(string path)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override DateTime GetCreationTime(string path) => fileSystem.File.GetCreationTime(path);

		public override DateTime GetCreationTimeUtc(string path) => fileSystem.File.GetCreationTimeUtc(path);

		public override string GetCurrentDirectory() => currentDirectory;

		public override string[] GetDirectories(string path) => GetDirectories(path, "*");

		public override string[] GetDirectories(string path, string searchPattern) => GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);

		public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption) => EnumerateDirectories(path, searchPattern, searchOption).ToArray();

		public override string GetDirectoryRoot(string path) => Path.GetPathRoot(path);

		public override string[] GetFiles(string path) => GetFiles(path, "*");

		public override string[] GetFiles(string path, string searchPattern) => GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);

		public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			if (!Exists(path))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, path));
			}

			return GetFilesInternal(fileSystem.AllFiles, path, searchPattern, searchOption);
		}

		private string[] GetFilesInternal(IEnumerable<string> files, string path, string searchPattern, SearchOption searchOption)
		{
			CheckSearchPattern(searchPattern);
			path = EnsurePathEndsWithDirectorySeparator(path);
			path = fileSystem.Path.GetFullPath(path);

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
						Regex.Escape(path),
						searchOption == SearchOption.AllDirectories ? allDirectoriesPattern : string.Empty,
						fileNamePatternSpecial);
				}
			}

			var pathPattern = string.Format(
				CultureInfo.InvariantCulture,
				isUnix ? @"(?i:^{0}{1}{2}(?:/?)$)" : @"(?i:^{0}{1}{2}(?:\\?)$)",
				Regex.Escape(path),
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

		public override string[] GetFileSystemEntries(string path)
		{
			return GetFileSystemEntries(path, "*");
		}

		public override string[] GetFileSystemEntries(string path, string searchPattern)
		{
			var dirs = GetDirectories(path, searchPattern);
			var files = GetFiles(path, searchPattern);

			return dirs.Union(files).ToArray();
		}

		public override DateTime GetLastAccessTime(string path) => fileSystem.File.GetLastAccessTime(path);

		public override DateTime GetLastAccessTimeUtc(string path) => fileSystem.File.GetLastAccessTimeUtc(path);

		public override DateTime GetLastWriteTime(string path) => fileSystem.File.GetLastWriteTime(path);

		public override DateTime GetLastWriteTimeUtc(string path) => fileSystem.File.GetLastWriteTimeUtc(path);

		public override string[] GetLogicalDrives()
		{
			return fileSystem
				.AllDirectories
				.Select(d => new MockDirectoryInfo(fileSystem, d).Root.FullName)
				.Select(r => r.ToLowerInvariant())
				.Distinct()
				.ToArray();
		}

		public override DirectoryInfoBase GetParent(string path)
		{
			if (path.Length == 0)
			{
				throw new ArgumentException(Properties.Resources.PATH_CANNOT_BE_THE_EMPTY_STRING_OR_ALL_WHITESPACE, nameof(path));
			}

			if (MockPath.HasIllegalCharacters(path, false))
			{
				throw new ArgumentException(Resources.INVALID_CHARACTERS, nameof(path));
			}

			var absolutePath = fileSystem.Path.GetFullPath(path);
			var sepAsString = fileSystem.Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

			var lastIndex = 0;
			if (absolutePath != sepAsString)
			{
				var startIndex = absolutePath.EndsWith(sepAsString, StringComparison.OrdinalIgnoreCase) ? absolutePath.Length - 1 : absolutePath.Length;
				lastIndex = absolutePath.LastIndexOf(fileSystem.Path.DirectorySeparatorChar, startIndex - 1);
				if (lastIndex < 0)
				{
					return null;
				}
			}

			var parentPath = absolutePath.Substring(0, lastIndex);
			if (string.IsNullOrEmpty(parentPath))
			{
				return null;
			}

			var parent = new MockDirectoryInfo(fileSystem, parentPath);
			return parent;
		}

		public override void Move(string sourceDirName, string destDirName)
		{
			var fullSourcePath = EnsurePathEndsWithDirectorySeparator(fileSystem.Path.GetFullPath(sourceDirName));
			var fullDestPath = EnsurePathEndsWithDirectorySeparator(fileSystem.Path.GetFullPath(destDirName));

			if (string.Equals(fullSourcePath, fullDestPath, StringComparison.OrdinalIgnoreCase))
			{
				throw new IOException("Source and destination path must be different.");
			}

			var sourceRoot = fileSystem.Path.GetPathRoot(fullSourcePath);
			var destinationRoot = fileSystem.Path.GetPathRoot(fullDestPath);
			if (!string.Equals(sourceRoot, destinationRoot, StringComparison.OrdinalIgnoreCase))
			{
				throw new IOException("Source and destination path must have identical roots. Move will not work across volumes.");
			}

			//Make sure that the destination exists
			fileSystem.Directory.CreateDirectory(fullDestPath);

			//Recursively move all the subdirectories from the source into the destination directory
			var subdirectories = GetDirectories(fullSourcePath);
			foreach (var subdirectory in subdirectories)
			{
				var newSubdirPath = subdirectory.Replace(fullSourcePath, fullDestPath, StringComparison.OrdinalIgnoreCase);
				Move(subdirectory, newSubdirPath);
			}

			//Move the files in destination directory
			var files = GetFiles(fullSourcePath);
			foreach (var file in files)
			{
				var newFilePath = file.Replace(fullSourcePath, fullDestPath, StringComparison.OrdinalIgnoreCase);
				fileSystem.FromFileName(file).MoveTo(newFilePath);
			}

			//Delete the source directory
			Delete(fullSourcePath);
		}

		public override void SetAccessControl(string path, DirectorySecurity directorySecurity)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override void SetCreationTime(string path, DateTime creationTime) => fileSystem.File.SetCreationTime(path, creationTime);

		public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc) => fileSystem.File.SetCreationTimeUtc(path, creationTimeUtc);

		public override void SetCurrentDirectory(string path) => currentDirectory = path;

		public override void SetLastAccessTime(string path, DateTime lastAccessTime) => fileSystem.File.SetLastAccessTime(path, lastAccessTime);

		public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc) => fileSystem.File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);

		public override void SetLastWriteTime(string path, DateTime lastWriteTime) => fileSystem.File.SetLastWriteTime(path, lastWriteTime);

		public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc) => fileSystem.File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);

		public override IEnumerable<string> EnumerateDirectories(string path)
		{
			validator.IsLegalAbsoluteOrRelative(path, "path");

			return EnumerateDirectories(path, "*");
		}

		public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
		{
			validator.IsLegalAbsoluteOrRelative(path, "path");

			return EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			validator.IsLegalAbsoluteOrRelative(path, "path");

			path = EnsurePathEndsWithDirectorySeparator(path);

			if (!Exists(path))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, path));
			}

			var dirs = GetFilesInternal(fileSystem.AllDirectories, path, searchPattern, searchOption);
			return dirs.Where(p => string.Compare(p, path, StringComparison.OrdinalIgnoreCase) != 0);
		}

		public override IEnumerable<string> EnumerateFiles(string path) => GetFiles(path);

		public override IEnumerable<string> EnumerateFiles(string path, string searchPattern) => GetFiles(path, searchPattern);

		public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption) => GetFiles(path, searchPattern, searchOption);

		public override IEnumerable<string> EnumerateFileSystemEntries(string path) => new List<string>(GetFiles(path).Concat( GetDirectories(path) ));

		public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern) => new List<string>(GetFiles(path, searchPattern).Concat( GetDirectories(path) ));

		public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption) => new List<string>(GetFiles(path, searchPattern, searchOption).Concat( GetDirectories(path, searchPattern, searchOption) ));

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
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION, "searchPattern");
			}
		}
	}
}