using DragonSpark.Extensions;
using DragonSpark.Sources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Provides access to the file system storage.
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockFile : FileBase
	{
		readonly private IFileSystemAccessor fileSystemAccessor;
		readonly private MockPath mockPath;

		public MockFile(IFileSystemAccessor fileSystemAccessor)
		{
			this.fileSystemAccessor = fileSystemAccessor;
			mockPath = new MockPath(fileSystemAccessor);
		}

		public override void AppendAllLines(string path, IEnumerable<string> contents)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
			AppendAllLines(path, contents, Defaults.DefaultEncoding);
		}

		public override void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			AppendAllText(path, contents.Aggregate(string.Empty, (a, b) => $"{a}{b}{Environment.NewLine}" ), encoding);
		}

		public override void AppendAllText(string path, string contents)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			AppendAllText(path, contents, Defaults.DefaultEncoding);
		}

		public override void AppendAllText(string path, string contents, Encoding encoding)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			if (!fileSystemAccessor.FileExists(path))
			{
				var dir = fileSystemAccessor.Path.GetDirectoryName(path);
				if (!fileSystemAccessor.Directory.Exists(dir))
				{
					throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, path));
				}

				fileSystemAccessor.AddFile(path, FileElement.Create(contents, encoding));
			}
			else
			{
				var file = fileSystemAccessor.GetFile( path );
				var bytesToAppend = encoding.GetBytes( contents );
				file.Assign( file.Get().Concat( bytesToAppend ) );
			}
		}

		public override StreamWriter AppendText(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			if (fileSystemAccessor.FileExists(path))
			{
				StreamWriter sw = new StreamWriter(OpenWrite(path));
				sw.BaseStream.Seek(0, SeekOrigin.End); //push the stream pointer at the end for append.
				return sw;
			}

			return new StreamWriter(Create(path));
		}

		public override void Copy(string sourceFileName, string destFileName)
		{
			Copy(sourceFileName, destFileName, false);
		}

		public override void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName", Properties.Resources.FILENAME_CANNOT_BE_NULL);
			}

			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName", Properties.Resources.FILENAME_CANNOT_BE_NULL);
			}

			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(sourceFileName, "sourceFileName");
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(destFileName, "destFileName");

			var directoryNameOfDestination = mockPath.GetDirectoryName(destFileName);
			if (!fileSystemAccessor.Directory.Exists(directoryNameOfDestination))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, destFileName));
			}

			var fileExists = fileSystemAccessor.FileExists(destFileName);
			if (fileExists)
			{
				if (!overwrite)
				{
					throw new IOException(string.Format(CultureInfo.InvariantCulture, "The file {0} already exists.", destFileName));
				}

				fileSystemAccessor.RemoveFile(destFileName);
			}

			var sourceFile = fileSystemAccessor.GetFile(sourceFileName);
			fileSystemAccessor.AddFile(destFileName, sourceFile);
		}

		public override Stream Create(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			fileSystemAccessor.AddFile(path, new FileElement(new byte[0]));
			var stream = OpenWrite(path);
			return stream;
		}

		public override Stream Create(string path, int bufferSize)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override Stream Create(string path, int bufferSize, FileOptions options)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override StreamWriter CreateText(string path)
		{
			return new StreamWriter(Create(path));
		}

		public override void Decrypt(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			new MockFileInfo(fileSystemAccessor, path).Decrypt();
		}

		public override void Delete(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			fileSystemAccessor.RemoveFile(path);
		}

		public override void Encrypt(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			new MockFileInfo(fileSystemAccessor, path).Encrypt();
		}

		public override bool Exists(string path)
		{
			return fileSystemAccessor.FileExists(path) && !fileSystemAccessor.AllDirectories.Any(d => d.Equals(path, StringComparison.OrdinalIgnoreCase));
		}

		public override FileSecurity GetAccessControl(string path)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		/// <summary>
		/// Gets the <see cref="FileAttributes"/> of the file on the path.
		/// </summary>
		/// <param name="path">The path to the file.</param>
		/// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
		/// <exception cref="ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters.</exception>
		/// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
		/// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
		/// <exception cref="FileNotFoundException"><paramref name="path"/> represents a file and is invalid, such as being on an unmapped drive, or the file cannot be found.</exception>
		/// <exception cref="DirectoryNotFoundException"><paramref name="path"/> represents a directory and is invalid, such as being on an unmapped drive, or the directory cannot be found.</exception>
		/// <exception cref="IOException">This file is being used by another process.</exception>
		/// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
		public override FileAttributes GetAttributes(string path)
		{
			if (path != null)
			{
				if (path.Length == 0)
				{
					throw new ArgumentException(Properties.Resources.THE_PATH_IS_NOT_OF_A_LEGAL_FORM, "path");
				}
			}

			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			var possibleFileData = fileSystemAccessor.GetElement(path);
			FileAttributes result;
			if (possibleFileData != null)
			{
				result = possibleFileData.Attributes;
			}
			else
			{
				var directoryInfo = fileSystemAccessor.DirectoryInfo.FromDirectoryName(path);
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
							Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, path));
					}

					throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find file '{0}'.", path));
				}
			}

			return result;
		}

		public override DateTime GetCreationTime(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return GetTimeFromFile(path, data => data.CreationTime.LocalDateTime, () => Defaults.DefaultDateTimeOffset.LocalDateTime);
		}

		public override DateTime GetCreationTimeUtc(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return GetTimeFromFile(path, data => data.CreationTime.UtcDateTime, () => Defaults.DefaultDateTimeOffset.UtcDateTime);
		}

		public override DateTime GetLastAccessTime(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return GetTimeFromFile(path, data => data.LastAccessTime.LocalDateTime, () => Defaults.DefaultDateTimeOffset.LocalDateTime);
		}

		public override DateTime GetLastAccessTimeUtc(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return GetTimeFromFile(path, data => data.LastAccessTime.UtcDateTime, () => Defaults.DefaultDateTimeOffset.UtcDateTime);
		}

		public override DateTime GetLastWriteTime(string path) {
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return GetTimeFromFile(path, data => data.LastWriteTime.LocalDateTime, () => Defaults.DefaultDateTimeOffset.LocalDateTime);
		}

		public override DateTime GetLastWriteTimeUtc(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return GetTimeFromFile(path, data => data.LastWriteTime.UtcDateTime, () => Defaults.DefaultDateTimeOffset.UtcDateTime);
		}

		private DateTime GetTimeFromFile(string path, Func<IFileElement, DateTime> existingFileFunction, Func<DateTime> nonExistingFileFunction)
		{
			var file = fileSystemAccessor.GetFile(path);
			var result = file != null ? existingFileFunction(file) : nonExistingFileFunction();
			return result;
		}

		public override void Move(string sourceFileName, string destFileName)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(sourceFileName, "sourceFileName");
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(destFileName, "destFileName");

			if (fileSystemAccessor.GetElement(destFileName) != null)
				throw new IOException("A file can not be created if it already exists.");

			var sourceFile = fileSystemAccessor.GetFile(sourceFileName);

			if (sourceFile == null)
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "The file \"{0}\" could not be found.", sourceFileName), sourceFileName);

			var destDir = fileSystemAccessor.Directory.GetParent(destFileName);
			if (!destDir.Exists)
			{
				throw new DirectoryNotFoundException("Could not find a part of the path.");
			}

			fileSystemAccessor.AddFile(destFileName, new FileElement(sourceFile.ToArray()));
			fileSystemAccessor.RemoveFile(sourceFileName);
		}

		public override Stream Open(string path, FileMode mode)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return Open(path, mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite), FileShare.None);
		}

		public override Stream Open(string path, FileMode mode, FileAccess access)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return Open(path, mode, access, FileShare.None);
		}

		public override Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			bool exists = fileSystemAccessor.FileExists(path);

			if (mode == FileMode.CreateNew && exists)
				throw new IOException(string.Format(CultureInfo.InvariantCulture, "The file '{0}' already exists.", path));

			if ((mode == FileMode.Open || mode == FileMode.Truncate) && !exists)
				throw new FileNotFoundException(path);

			if (!exists || mode == FileMode.CreateNew)
				return Create(path);

			if (mode == FileMode.Create || mode == FileMode.Truncate)
			{
				Delete(path);
				return Create(path);
			}

			var length = fileSystemAccessor.GetFile(path).Get().Length;
			var stream = OpenWrite(path);

			if (mode == FileMode.Append)
				stream.Seek(length, SeekOrigin.Begin);

			return stream;
		}

		public override Stream OpenRead(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public override StreamReader OpenText(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return new StreamReader(
				OpenRead(path));
		}

		public override Stream OpenWrite(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return new MockFileStream(fileSystemAccessor, path);
		}

		public override byte[] ReadAllBytes(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return fileSystemAccessor.GetFile(path).ToArray();
		}

		public override string[] ReadAllLines(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			if (!fileSystemAccessor.FileExists(path))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
			}

			return fileSystemAccessor.GetFile(path).AsText().SplitLines();
		}

		public override string[] ReadAllLines(string path, Encoding encoding)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}

			if (!fileSystemAccessor.FileExists(path))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
			}

			return encoding.GetString(fileSystemAccessor.GetFile(path).ToArray()).SplitLines();
		}

		public override string ReadAllText(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			if (!fileSystemAccessor.FileExists(path))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
			}

			return ReadAllText(path, Defaults.DefaultEncoding);
		}

		public override string ReadAllText(string path, Encoding encoding)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}

			return ReadAllTextInternal(path, encoding);
		}

		public override IEnumerable<string> ReadLines(string path)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			return ReadAllLines(path);
		}

		public override IEnumerable<string> ReadLines(string path, Encoding encoding)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
			return ReadAllLines(path, encoding);
		}

		public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override void SetAccessControl(string path, FileSecurity fileSecurity)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override void SetAttributes(string path, FileAttributes fileAttributes)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			fileSystemAccessor.GetElement(path).Attributes = fileAttributes;
		}

		public override void SetCreationTime(string path, DateTime creationTime)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			fileSystemAccessor.GetElement(path).CreationTime = new DateTimeOffset(creationTime);
		}

		public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			fileSystemAccessor.GetElement(path).CreationTime = new DateTimeOffset(creationTimeUtc, TimeSpan.Zero);
		}

		public override void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			fileSystemAccessor.GetElement(path).LastAccessTime = new DateTimeOffset(lastAccessTime);
		}

		public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			fileSystemAccessor.GetElement(path).LastAccessTime = new DateTimeOffset(lastAccessTimeUtc, TimeSpan.Zero);
		}

		public override void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			fileSystemAccessor.GetElement(path).LastWriteTime = new DateTimeOffset(lastWriteTime);
		}

		public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			fileSystemAccessor.GetElement(path).LastWriteTime = new DateTimeOffset(lastWriteTimeUtc, TimeSpan.Zero);
		}

		/// <summary>
		/// Creates a new file, writes the specified byte array to the file, and then closes the file.
		/// If the target file already exists, it is overwritten.
		/// </summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="bytes">The bytes to write to the file. </param>
		/// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/> or contents is empty.</exception>
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
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <remarks>
		/// Given a byte array and a file path, this method opens the specified file, writes the contents of the byte array to the file, and then closes the file.
		/// </remarks>
		public override void WriteAllBytes(string path, byte[] bytes) => fileSystemAccessor.AddFile(path, new FileElement(bytes));

		/// <summary>
		/// Creates a new file, writes a collection of strings to the file, and then closes the file.
		/// </summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The lines to write to the file.</param>
		/// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="path"/> specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// <paramref name="path"/> specified a directory.
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
		public override void WriteAllLines(string path, IEnumerable<string> contents)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
			WriteAllLines(path, contents, Defaults.DefaultEncoding);
		}

		/// <summary>
		/// Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
		/// </summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The lines to write to the file.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException">Either <paramref name="path"/>, <paramref name="contents"/>, or <paramref name="encoding"/> is <see langword="null"/>.</exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="path"/> specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// <paramref name="path"/> specified a directory.
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
		public override void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
			var sb = new StringBuilder();
			foreach (var line in contents)
			{
				sb.AppendLine(line);
			}

			WriteAllText(path, sb.ToString(), encoding);
		}

		/// <summary>
		/// Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.
		/// </summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The string array to write to the file.</param>
		/// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="path"/> specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// <paramref name="path"/> specified a directory.
		/// -or-
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
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
		public override void WriteAllLines(string path, string[] contents)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
			WriteAllLines(path, contents, Defaults.DefaultEncoding);
		}

		/// <summary>
		/// Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.
		/// </summary>
		/// <param name="path">The file to write to.</param>
		/// <param name="contents">The string array to write to the file.</param>
		/// <param name="encoding">An <see cref="Encoding"/> object that represents the character encoding applied to the string array.</param>
		/// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length.
		/// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="path"/> specified a file that is read-only.
		/// -or-
		/// This operation is not supported on the current platform.
		/// -or-
		/// <paramref name="path"/> specified a directory.
		/// -or-
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
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
		public override void WriteAllLines(string path, string[] contents, Encoding encoding)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
			WriteAllLines(path, new List<string>(contents), encoding);
		}

		/// <summary>
		/// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		/// <param name="path">The file to write to. </param>
		/// <param name="contents">The string to write to the file. </param>
		/// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/> or contents is empty.</exception>
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
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <remarks>
		/// This method uses UTF-8 encoding without a Byte-Order Mark (BOM), so using the <see cref="M:Encoding.GetPreamble"/> method will return an empty byte array.
		/// If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file, use the <see cref="FileBase.WriteAllText(string,string,System.Text.Encoding)"/> method overload with <see cref="UTF8Encoding"/> encoding.
		/// <para>
		/// Given a string and a file path, this method opens the specified file, writes the string to the file, and then closes the file.
		/// </para>
		/// </remarks>
		public override void WriteAllText(string path, string contents)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

			WriteAllText(path, contents, Defaults.DefaultEncoding);
		}

		/// <summary>
		/// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		/// <param name="path">The file to write to. </param>
		/// <param name="contents">The string to write to the file. </param>
		/// <param name="encoding">The encoding to apply to the string.</param>
		/// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/> or contents is empty.</exception>
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
		/// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
		/// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
		/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <remarks>
		/// Given a string and a file path, this method opens the specified file, writes the string to the file using the specified encoding, and then closes the file.
		/// The file handle is guaranteed to be closed by this method, even if exceptions are raised.
		/// </remarks>
		public override void WriteAllText(string path, string contents, Encoding encoding)
		{
			fileSystemAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
			
			if (fileSystemAccessor.Directory.Exists(path))
			{
				throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, path));
			}

			var data = contents == null ? FileElement.Empty() : FileElement.Create(contents, encoding);
			fileSystemAccessor.AddFile(path, data);
		}

		internal static string ReadAllBytes(byte[] contents, Encoding encoding)
		{
			using (var ms = new MemoryStream(contents))
			using (var sr = new StreamReader(ms, encoding))
			{
				return sr.ReadToEnd();
			}
		}

		private string ReadAllTextInternal(string path, Encoding encoding)
		{
			var mockFileData = fileSystemAccessor.GetFile(path);
			return ReadAllBytes(mockFileData.ToArray(), encoding);
		}
	}
}