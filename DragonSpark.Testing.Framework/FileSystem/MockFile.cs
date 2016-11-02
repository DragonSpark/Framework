using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Windows.FileSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Provides access to the file system storage.
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockFile : FileBase
	{
		readonly IFileSystemRepository repository;
		readonly IPath path;
		readonly IDirectory directory;

		public MockFile() : this( FileSystemRepository.Default, Windows.FileSystem.Path.Default, Windows.FileSystem.Directory.Default ) {}

		public MockFile(IFileSystemRepository repository, IPath path, IDirectory directory )
		{
			this.repository = repository;
			this.path = path;
			this.directory = directory;
		}

		public override void AppendAllLines(string pathName, IEnumerable<string> contents)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			AppendAllLines(pathName, contents, Defaults.DefaultEncoding);
		}

		public override void AppendAllLines(string pathName, IEnumerable<string> contents, Encoding encoding)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			AppendAllText(pathName, contents.Aggregate(string.Empty, (a, b) => $"{a}{b}{Environment.NewLine}" ), encoding);
		}

		public override void AppendAllText(string pathName, string contents)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			AppendAllText(pathName, contents, Defaults.DefaultEncoding);
		}

		public override void AppendAllText(string pathName, string contents, Encoding encoding)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			if (!repository.Contains(pathName))
			{
				var dir = path.GetDirectoryName(pathName);
				if (!directory.Exists(dir))
				{
					throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, pathName));
				}

				repository.Set( pathName, FileElement.Create( contents, encoding ) );
			}
			else
			{
				var file = repository.GetFile( pathName );
				var bytesToAppend = encoding.GetBytes( contents );
				file.Assign( file.Get().Concat( bytesToAppend ) );
			}
		}

		public override StreamWriter AppendText(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			if (repository.Contains(pathName))
			{
				StreamWriter sw = new StreamWriter(OpenWrite(pathName));
				sw.BaseStream.Seek(0, SeekOrigin.End); //push the stream pointer at the end for append.
				return sw;
			}

			return new StreamWriter(Create(pathName));
		}

		public override void Copy(string sourceFileName, string destFileName)
		{
			Copy(sourceFileName, destFileName, false);
		}

		public override void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			path.IsLegalAbsoluteOrRelative(sourceFileName, "sourceFileName");
			path.IsLegalAbsoluteOrRelative(destFileName, "destFileName");

			var directoryNameOfDestination = path.GetDirectoryName(destFileName);
			if (!directory.Exists(directoryNameOfDestination))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, destFileName));
			}

			var fileExists = repository.Contains(destFileName);
			if (fileExists)
			{
				if (!overwrite)
				{
					throw new IOException(string.Format(CultureInfo.InvariantCulture, "The file {0} already exists.", destFileName));
				}

				repository.Remove(destFileName);
			}

			var sourceFile = repository.GetFile(sourceFileName);
			repository.Set( destFileName, new FileElement( sourceFile.Unwrap() ) );
		}

		public override Stream Create(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			repository.Set( pathName, FileElement.Empty() );
			var stream = OpenWrite(pathName);
			return stream;
		}

		public override Stream Create(string pathName, int bufferSize)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override Stream Create(string pathName, int bufferSize, FileOptions options)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override Stream Create(string pathName, int bufferSize, FileOptions options, FileSecurity fileSecurity)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override StreamWriter CreateText(string pathName) => new StreamWriter(Create(pathName));

		public override void Decrypt(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			repository.FromFileName(pathName).Decrypt();
		}

		public override void Delete(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			repository.Remove(pathName);
		}

		public override void Encrypt(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			repository.FromFileName(pathName).Encrypt();
		}

		public override bool Exists(string pathName) => repository.Contains(pathName) && !repository.AllDirectories.Any(d => d.Equals(pathName, StringComparison.OrdinalIgnoreCase));

		public override FileSecurity GetAccessControl(string pathName)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override FileSecurity GetAccessControl(string pathName, AccessControlSections includeSections)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		/// <summary>
		/// Gets the <see cref="FileAttributes"/> of the file on the path.
		/// </summary>
		/// <param name="pathName">The path to the file.</param>
		/// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
		/// <exception cref="ArgumentException"><paramref name="pathName"/> is empty, contains only white spaces, or contains invalid characters.</exception>
		/// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
		/// <exception cref="NotSupportedException"><paramref name="pathName"/> is in an invalid format.</exception>
		/// <exception cref="FileNotFoundException"><paramref name="pathName"/> represents a file and is invalid, such as being on an unmapped drive, or the file cannot be found.</exception>
		/// <exception cref="DirectoryNotFoundException"><paramref name="pathName"/> represents a directory and is invalid, such as being on an unmapped drive, or the directory cannot be found.</exception>
		/// <exception cref="IOException">This file is being used by another process.</exception>
		/// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
		public override FileAttributes GetAttributes(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			var possibleFileData = repository.Get(pathName);
			FileAttributes result;
			if (possibleFileData != null)
			{
				result = possibleFileData.Attributes;
			}
			else
			{
				var directoryInfo = repository.FromDirectoryName(pathName);
				if (directoryInfo.Exists)
				{
					result = directoryInfo.Attributes;
				}
				else
				{
					var parentDirectoryInfo = directoryInfo.Parent;
					if (!parentDirectoryInfo.Exists)
					{
						throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture,
							Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, pathName));
					}

					throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find file '{0}'.", pathName));
				}
			}

			return result;
		}

		public override DateTime GetCreationTime(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return GetTimeFromFile(pathName, data => data.CreationTime.LocalDateTime, () => Defaults.DefaultDateTimeOffset.LocalDateTime);
		}

		public override DateTime GetCreationTimeUtc(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return GetTimeFromFile(pathName, data => data.CreationTime.UtcDateTime, () => Defaults.DefaultDateTimeOffset.UtcDateTime);
		}

		public override DateTime GetLastAccessTime(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return GetTimeFromFile(pathName, data => data.LastAccessTime.LocalDateTime, () => Defaults.DefaultDateTimeOffset.LocalDateTime);
		}

		public override DateTime GetLastAccessTimeUtc(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return GetTimeFromFile(pathName, data => data.LastAccessTime.UtcDateTime, () => Defaults.DefaultDateTimeOffset.UtcDateTime);
		}

		public override DateTime GetLastWriteTime(string pathName) {
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return GetTimeFromFile(pathName, data => data.LastWriteTime.LocalDateTime, () => Defaults.DefaultDateTimeOffset.LocalDateTime);
		}

		public override DateTime GetLastWriteTimeUtc(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return GetTimeFromFile(pathName, data => data.LastWriteTime.UtcDateTime, () => Defaults.DefaultDateTimeOffset.UtcDateTime);
		}

		DateTime GetTimeFromFile(string pathName, Func<IFileElement, DateTime> existingFileFunction, Func<DateTime> nonExistingFileFunction)
		{
			var file = repository.GetFile(pathName);
			var result = file != null ? existingFileFunction(file) : nonExistingFileFunction();
			return result;
		}

		public override void Move(string sourceFileName, string destFileName)
		{
			path.IsLegalAbsoluteOrRelative(sourceFileName, nameof(sourceFileName));
			path.IsLegalAbsoluteOrRelative(destFileName, nameof(destFileName));

			if (repository.Get(destFileName) != null)
				throw new IOException("A file can not be created if it already exists.");

			var sourceFile = repository.GetFile( sourceFileName );

			if ( sourceFile == null )
				throw new FileNotFoundException( string.Format( CultureInfo.InvariantCulture, @"The file ""{0}"" could not be found.", sourceFileName ), sourceFileName );

			var destDir = directory.GetParent( destFileName );
			if ( !destDir.Exists )
			{
				throw new DirectoryNotFoundException( "Could not find a part of the path." );
			}

			repository.Set( destFileName, new FileElement( sourceFile.Unwrap() ) );
			repository.Remove( sourceFileName );
		}

		public override Stream Open(string pathName, FileMode mode)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return Open(pathName, mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite), FileShare.None);
		}

		public override Stream Open(string pathName, FileMode mode, FileAccess access)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return Open(pathName, mode, access, FileShare.None);
		}

		public override Stream Open(string pathName, FileMode mode, FileAccess access, FileShare share)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			bool exists = repository.Contains(pathName);

			if (mode == FileMode.CreateNew && exists)
				throw new IOException(string.Format(CultureInfo.InvariantCulture, "The file '{0}' already exists.", pathName));

			if ((mode == FileMode.Open || mode == FileMode.Truncate) && !exists)
				throw new FileNotFoundException(pathName);

			if (!exists || mode == FileMode.CreateNew)
				return Create(pathName);

			if (mode == FileMode.Create || mode == FileMode.Truncate)
			{
				Delete(pathName);
				return Create(pathName);
			}

			var length = repository.GetFile(pathName).Get().Length;
			var stream = OpenWrite(pathName);

			if (mode == FileMode.Append)
				stream.Seek(length, SeekOrigin.Begin);

			return stream;
		}

		public override Stream OpenRead(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return Open(pathName, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public override StreamReader OpenText(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return new StreamReader(OpenRead(pathName));
		}

		public override Stream OpenWrite(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return new MockFileStream(repository, pathName);
		}

		public override byte[] ReadAllBytes(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return repository.GetFile(pathName).Unwrap();
		}

		public override string[] ReadAllLines(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			if (!repository.Contains(pathName))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", pathName));
			}

			return repository.GetFile(pathName).AsText().SplitLines();
		}

		public override string[] ReadAllLines(string pathName, Encoding encoding)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			if (encoding == null)
			{
				throw new ArgumentNullException(nameof( encoding ));
			}

			if (!repository.Contains(pathName))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", pathName));
			}

			return encoding.GetString(repository.GetFile(pathName).Unwrap()).SplitLines();
		}

		public override string ReadAllText(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			if (!repository.Contains(pathName))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", pathName));
			}

			return ReadAllText(pathName, Defaults.DefaultEncoding);
		}

		public override string ReadAllText(string pathName, Encoding encoding)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			if (encoding == null)
			{
				throw new ArgumentNullException(nameof( encoding ));
			}

			return ReadAllTextInternal(pathName, encoding);
		}

		public override IEnumerable<string> ReadLines(string pathName)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			return ReadAllLines(pathName);
		}

		public override IEnumerable<string> ReadLines(string pathName, Encoding encoding)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			return ReadAllLines(pathName, encoding);
		}

		public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override void SetAccessControl(string pathName, FileSecurity fileSecurity)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override void SetAttributes(string pathName, FileAttributes fileAttributes)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			repository.Get(pathName).Attributes = fileAttributes;
		}

		public override void SetCreationTime(string pathName, DateTime creationTime)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			repository.Get(pathName).CreationTime = new DateTimeOffset(creationTime);
		}

		public override void SetCreationTimeUtc(string pathName, DateTime creationTimeUtc)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			repository.Get(pathName).CreationTime = new DateTimeOffset(creationTimeUtc, TimeSpan.Zero);
		}

		public override void SetLastAccessTime(string pathName, DateTime lastAccessTime)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			repository.Get(pathName).LastAccessTime = new DateTimeOffset(lastAccessTime);
		}

		public override void SetLastAccessTimeUtc(string pathName, DateTime lastAccessTimeUtc)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			repository.Get(pathName).LastAccessTime = new DateTimeOffset(lastAccessTimeUtc, TimeSpan.Zero);
		}

		public override void SetLastWriteTime(string pathName, DateTime lastWriteTime)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			repository.Get(pathName).LastWriteTime = new DateTimeOffset(lastWriteTime);
		}

		public override void SetLastWriteTimeUtc(string pathName, DateTime lastWriteTimeUtc)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			repository.Get(pathName).LastWriteTime = new DateTimeOffset(lastWriteTimeUtc, TimeSpan.Zero);
		}

		/// <summary>
		/// Creates a new file, writes the specified byte array to the file, and then closes the file.
		/// If the target file already exists, it is overwritten.
		/// </summary>
		/// <param name="pathName">The file to write to.</param>
		/// <param name="bytes">The bytes to write to the file. </param>
		/// <exception cref="ArgumentException"><paramref name="pathName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="System.IO.Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="pathName"/> is <see langword="null"/> or contents is empty.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// path specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// path specified a directory.
		/// -or-
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="pathName"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="pathName"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <remarks>
		/// Given a byte array and a file path, this method opens the specified file, writes the contents of the byte array to the file, and then closes the file.
		/// </remarks>
		public override void WriteAllBytes(string pathName, byte[] bytes) => repository.Set( pathName, new FileElement( bytes ) );

		/// <summary>
		/// Creates a new file, writes a collection of strings to the file, and then closes the file.
		/// </summary>
		/// <param name="pathName">The file to write to.</param>
		/// <param name="contents">The lines to write to the file.</param>
		/// <exception cref="ArgumentException"><paramref name="pathName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="System.IO.Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException">Either <paramref name="pathName"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="pathName"/> was not found.</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="NotSupportedException"><paramref name="pathName"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="pathName"/> specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// <paramref name="pathName"/> specified a directory.
		/// -or-
		/// The caller does not have the required permission.
		/// </exception>
		/// <remarks>
		/// <para>
		///     If the target file already exists, it is overwritten.
		/// </para>
		/// <para>
		///     You can use this method to create the contents for a collection class that takes an <see cref="IEnumerable{T}"/> in its constructor, such as a <see cref="List{T}"/>, <see cref="HashSet{T}"/>, or a <see cref="SortedSet{T}"/> class.
		/// </para>
		/// </remarks>
		public override void WriteAllLines(string pathName, IEnumerable<string> contents)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			WriteAllLines(pathName, contents, Defaults.DefaultEncoding);
		}

		/// <summary>
		/// Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
		/// </summary>
		/// <param name="pathName">The file to write to.</param>
		/// <param name="contents">The lines to write to the file.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="ArgumentException"><paramref name="pathName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="System.IO.Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException">Either <paramref name="pathName"/>, <paramref name="contents"/>, or <paramref name="encoding"/> is <see langword="null"/>.</exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="pathName"/> was not found.</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="NotSupportedException"><paramref name="pathName"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="pathName"/> specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// <paramref name="pathName"/> specified a directory.
		/// -or-
		/// The caller does not have the required permission.
		/// </exception>
		/// <remarks>
		/// <para>
		///     If the target file already exists, it is overwritten.
		/// </para>
		/// <para>
		///     You can use this method to create a file that contains the following:
		/// <list type="bullet">
		/// <item>
		/// <description>The results of a LINQ to Objects query on the lines of a file, as obtained by using the ReadLines method.</description>
		/// </item>
		/// <item>
		/// <description>The contents of a collection that implements an <see cref="IEnumerable{T}"/> of strings.</description>
		/// </item>
		/// </list>
		/// </para>
		/// </remarks>
		public override void WriteAllLines(string pathName, IEnumerable<string> contents, Encoding encoding)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			var sb = new StringBuilder();
			foreach (var line in contents)
			{
				sb.AppendLine(line);
			}

			WriteAllText(pathName, sb.ToString(), encoding);
		}

		/// <summary>
		/// Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.
		/// </summary>
		/// <param name="pathName">The file to write to.</param>
		/// <param name="contents">The string array to write to the file.</param>
		/// <exception cref="ArgumentException"><paramref name="pathName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="System.IO.Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException">Either <paramref name="pathName"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="pathName"/> specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// <paramref name="pathName"/> specified a directory.
		/// -or-
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="pathName"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="pathName"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <remarks>
		/// <para>
		///     If the target file already exists, it is overwritten.
		/// </para>
		/// <para>
		///     The default behavior of the WriteAllLines method is to write out data using UTF-8 encoding without a byte order mark (BOM). If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file, use the <see cref="FileBase.WriteAllLines(string,string[],System.Text.Encoding)"/> method overload with <see cref="UTF8Encoding"/> encoding.
		/// </para>
		/// <para>
		///     Given a string array and a file path, this method opens the specified file, writes the string array to the file using the specified encoding,
		///     and then closes the file.
		/// </para>
		/// </remarks>
		public override void WriteAllLines(string pathName, string[] contents)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			WriteAllLines(pathName, contents, Defaults.DefaultEncoding);
		}

		/// <summary>
		/// Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.
		/// </summary>
		/// <param name="pathName">The file to write to.</param>
		/// <param name="contents">The string array to write to the file.</param>
		/// <param name="encoding">An <see cref="Encoding"/> object that represents the character encoding applied to the string array.</param>
		/// <exception cref="ArgumentException"><paramref name="pathName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="System.IO.Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException">Either <paramref name="pathName"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="pathName"/> specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// <paramref name="pathName"/> specified a directory.
		/// -or-
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="pathName"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="pathName"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <remarks>
		/// <para>
		///     If the target file already exists, it is overwritten.
		/// </para>
		/// <para>
		///     Given a string array and a file path, this method opens the specified file, writes the string array to the file using the specified encoding,
		///     and then closes the file.
		/// </para>
		/// </remarks>
		public override void WriteAllLines(string pathName, string[] contents, Encoding encoding)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			WriteAllLines(pathName, new List<string>(contents), encoding);
		}

		/// <summary>
		/// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		/// <param name="pathName">The file to write to. </param>
		/// <param name="contents">The string to write to the file. </param>
		/// <exception cref="ArgumentException"><paramref name="pathName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="System.IO.Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="pathName"/> is <see langword="null"/> or contents is empty.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// path specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// path specified a directory.
		/// -or-
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="pathName"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="pathName"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <remarks>
		/// This method uses UTF-8 encoding without a Byte-Order Mark (BOM), so using the <see cref="M:Encoding.GetPreamble"/> method will return an empty byte array.
		/// If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file, use the <see cref="FileBase.WriteAllText(string,string,System.Text.Encoding)"/> method overload with <see cref="UTF8Encoding"/> encoding.
		/// <para>
		/// Given a string and a file path, this method opens the specified file, writes the string to the file, and then closes the file.
		/// </para>
		/// </remarks>
		public override void WriteAllText(string pathName, string contents)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));

			WriteAllText(pathName, contents, Defaults.DefaultEncoding);
		}

		/// <summary>
		/// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		/// <param name="pathName">The file to write to. </param>
		/// <param name="contents">The string to write to the file. </param>
		/// <param name="encoding">The encoding to apply to the string.</param>
		/// <exception cref="ArgumentException"><paramref name="pathName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="System.IO.Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="pathName"/> is <see langword="null"/> or contents is empty.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// path specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// path specified a directory.
		/// -or-
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="pathName"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="pathName"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <remarks>
		/// Given a string and a file path, this method opens the specified file, writes the string to the file using the specified encoding, and then closes the file.
		/// The file handle is guaranteed to be closed by this method, even if exceptions are raised.
		/// </remarks>
		public override void WriteAllText(string pathName, string contents, Encoding encoding)
		{
			path.IsLegalAbsoluteOrRelative(pathName, nameof(pathName));
			
			if (directory.Exists(pathName))
			{
				throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, pathName));
			}

			var data = contents == null ? FileElement.Empty() : FileElement.Create( contents, encoding );
			repository.Set( pathName, data );
		}

		internal static string ReadAllBytes(byte[] contents, Encoding encoding)
		{
			using (var ms = new MemoryStream(contents))
			using (var sr = new StreamReader(ms, encoding))
			{
				return sr.ReadToEnd();
			}
		}

		private string ReadAllTextInternal(string pathName, Encoding encoding) => ReadAllBytes(repository.GetFile(pathName).Unwrap(), encoding);
	}
}