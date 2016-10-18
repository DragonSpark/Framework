using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockDirectoryInfo : DirectoryInfoBase
	{
		readonly IFileSystemAccessor fileSystemAccessor;
		readonly string directoryPath;
		readonly IDirectoryElement element;

		/// <summary>
		/// Initializes a new instance of the <see cref="MockDirectoryInfo"/> class.
		/// </summary>
		/// <param name="fileSystemAccessor">The mock file data accessor.</param>
		/// <param name="directoryPath">The directory path.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="fileSystemAccessor"/> or <paramref name="directoryPath"/> is <see langref="null"/>.</exception>
		public MockDirectoryInfo(IFileSystemAccessor fileSystemAccessor, string directoryPath)
		{
			this.fileSystemAccessor = fileSystemAccessor;

			this.directoryPath = EnsurePathEndsWithDirectorySeparator(fileSystemAccessor.Path.GetFullPath(directoryPath));
			element = (IDirectoryElement)fileSystemAccessor.GetElement( directoryPath );
			if (element == null) throw new FileNotFoundException("File not found", directoryPath);
		}

		public override void Delete() => fileSystemAccessor.Directory.Delete(directoryPath);

		public override void Refresh() {}

		public override FileAttributes Attributes
		{
			get { return element.Attributes; }
			set { element.Attributes = value; }
		}

		public override DateTime CreationTime
		{
			get { return element.CreationTime.DateTime; }
			set { element.CreationTime = value; }
		}

		public override DateTime CreationTimeUtc
		{
			get { return element.CreationTime.UtcDateTime; }
			set { element.CreationTime = value.ToLocalTime(); }
		}

		public override bool Exists => fileSystemAccessor.Directory.Exists(FullName);

		// System.IO.Path.GetExtension does only string manipulation,
		// so it's safe to delegate.
		public override string Extension => Path.GetExtension(directoryPath);

		public override string FullName
		{
			get
			{
				var root = fileSystemAccessor.Path.GetPathRoot(directoryPath);
				// directories do not have a trailing slash
				return string.Equals(directoryPath, root, StringComparison.OrdinalIgnoreCase) ? directoryPath : directoryPath.TrimEnd('\\').TrimEnd('/');
			}
		}

		public override DateTime LastAccessTime
		{
			get { return element.LastAccessTime.DateTime; }
			set { element.LastAccessTime = value; }
		}

		public override DateTime LastAccessTimeUtc
		{
			get { return element.LastAccessTime.UtcDateTime; }
			set { element.LastAccessTime = value.ToLocalTime(); }
		}

		public override DateTime LastWriteTime
		{
			get { return element.LastWriteTime.DateTime; }
			set { element.LastWriteTime = value; }
		}

		public override DateTime LastWriteTimeUtc
		{
			get { return element.LastWriteTime.UtcDateTime; }
			set { element.LastWriteTime = value.ToLocalTime(); }
		}

		public override string Name => new MockPath(fileSystemAccessor).GetFileName(directoryPath.TrimEnd('\\'));

		public override void Create() => fileSystemAccessor.Directory.CreateDirectory(FullName);

		public override void Create(DirectorySecurity directorySecurity) => fileSystemAccessor.Directory.CreateDirectory(FullName, directorySecurity);

		public override DirectoryInfoBase CreateSubdirectory(string path) => fileSystemAccessor.Directory.CreateDirectory(Path.Combine(FullName, path));

		public override DirectoryInfoBase CreateSubdirectory(string path, DirectorySecurity directorySecurity) => fileSystemAccessor.Directory.CreateDirectory(Path.Combine(FullName, path), directorySecurity);

		public override void Delete(bool recursive) => fileSystemAccessor.Directory.Delete(directoryPath, recursive);

		public override IEnumerable<DirectoryInfoBase> EnumerateDirectories() => GetDirectories();

		public override IEnumerable<DirectoryInfoBase> EnumerateDirectories(string searchPattern) => GetDirectories(searchPattern);

		public override IEnumerable<DirectoryInfoBase> EnumerateDirectories(string searchPattern, SearchOption searchOption) => GetDirectories(searchPattern, searchOption);

		public override IEnumerable<FileInfoBase> EnumerateFiles() => GetFiles();

		public override IEnumerable<FileInfoBase> EnumerateFiles(string searchPattern) => GetFiles(searchPattern);

		public override IEnumerable<FileInfoBase> EnumerateFiles(string searchPattern, SearchOption searchOption) => GetFiles(searchPattern, searchOption);

		public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos() => GetFileSystemInfos();

		public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos(string searchPattern) => GetFileSystemInfos(searchPattern);

		public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption) => GetFileSystemInfos(searchPattern, searchOption);

		public override DirectorySecurity GetAccessControl() => fileSystemAccessor.Directory.GetAccessControl(directoryPath);

		public override DirectorySecurity GetAccessControl(AccessControlSections includeSections) => fileSystemAccessor.Directory.GetAccessControl(directoryPath, includeSections);

		public override DirectoryInfoBase[] GetDirectories() => ConvertStringsToDirectories(fileSystemAccessor.Directory.GetDirectories(directoryPath));

		public override DirectoryInfoBase[] GetDirectories(string searchPattern) => ConvertStringsToDirectories(fileSystemAccessor.Directory.GetDirectories(directoryPath, searchPattern));

		public override DirectoryInfoBase[] GetDirectories(string searchPattern, SearchOption searchOption) => ConvertStringsToDirectories(fileSystemAccessor.Directory.GetDirectories(directoryPath, searchPattern, searchOption));

		DirectoryInfoBase[] ConvertStringsToDirectories(IEnumerable<string> paths) => paths.Select(path => new MockDirectoryInfo(fileSystemAccessor, path)).Cast<DirectoryInfoBase>().ToArray();

		public override FileInfoBase[] GetFiles() => ConvertStringsToFiles(fileSystemAccessor.Directory.GetFiles(FullName));

		public override FileInfoBase[] GetFiles(string searchPattern) => ConvertStringsToFiles(fileSystemAccessor.Directory.GetFiles(FullName, searchPattern));

		public override FileInfoBase[] GetFiles(string searchPattern, SearchOption searchOption) => ConvertStringsToFiles(fileSystemAccessor.Directory.GetFiles(FullName, searchPattern, searchOption));

		FileInfoBase[] ConvertStringsToFiles(IEnumerable<string> paths) => paths.Select(fileSystemAccessor.FileInfo.FromFileName).ToArray();

		public override FileSystemInfoBase[] GetFileSystemInfos() => GetFileSystemInfos("*");

		public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern) => GetFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);

		public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern, SearchOption searchOption) => GetDirectories(searchPattern, searchOption).OfType<FileSystemInfoBase>().Concat(GetFiles(searchPattern, searchOption)).ToArray();

		public override void MoveTo(string destDirName) => fileSystemAccessor.Directory.Move(directoryPath, destDirName);

		public override void SetAccessControl(DirectorySecurity directorySecurity) => fileSystemAccessor.Directory.SetAccessControl(directoryPath, directorySecurity);

		public override DirectoryInfoBase Parent => fileSystemAccessor.Directory.GetParent(directoryPath);

		public override DirectoryInfoBase Root => new MockDirectoryInfo(fileSystemAccessor, fileSystemAccessor.Directory.GetDirectoryRoot(FullName));

		private string EnsurePathEndsWithDirectorySeparator(string path)
		{
			if (!path.EndsWith(fileSystemAccessor.Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
			{
				path += fileSystemAccessor.Path.DirectorySeparatorChar;
			}

			return path;
		}
	}
}
