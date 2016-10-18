using DragonSpark.Sources;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockFileInfo : FileInfoBase
	{
		readonly IFileSystemAccessor accessor;
		string path;
		readonly IFileElement fileElement;

		public MockFileInfo(IFileSystemAccessor accessor, string path)
		{
			this.accessor = accessor;
			this.path = path;
			fileElement = accessor.GetFile( path );
		}

		public override void Delete() => accessor.RemoveFile(path);

		public override void Refresh() {}

		public override FileAttributes Attributes
		{
			get
			{
				if (fileElement == null)
					throw new FileNotFoundException("File not found", path);
				return fileElement.Attributes;
			}
			set { fileElement.Attributes = value; }
		}

		public override DateTime CreationTime
		{
			get
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				return fileElement.CreationTime.DateTime;
			}
			set
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				fileElement.CreationTime = value;
			}
		}

		public override DateTime CreationTimeUtc
		{
			get
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				return fileElement.CreationTime.UtcDateTime;
			}
			set
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				fileElement.CreationTime = value.ToLocalTime();
			}
		}

		public override bool Exists => fileElement != null;

		// System.IO.Path.GetExtension does only string manipulation,
		// so it's safe to delegate.
		public override string Extension => Path.GetExtension(path);

		public override string FullName => path;

		public override DateTime LastAccessTime
		{
			get
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				return fileElement.LastAccessTime.DateTime;
			}
			set
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				fileElement.LastAccessTime = value;
			}
		}

		public override DateTime LastAccessTimeUtc
		{
			get
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				return fileElement.LastAccessTime.UtcDateTime;
			}
			set
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				fileElement.LastAccessTime = value;
			}
		}

		public override DateTime LastWriteTime
		{
			get
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				return fileElement.LastWriteTime.DateTime;
			}
			set
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				fileElement.LastWriteTime = value;
			}
		}

		public override DateTime LastWriteTimeUtc
		{
			get
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				return fileElement.LastWriteTime.UtcDateTime;
			}
			set
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				fileElement.LastWriteTime = value.ToLocalTime();
			}
		}

		public override string Name => new MockPath(accessor).GetFileName(path);

		public override StreamWriter AppendText()
		{
			if (fileElement == null) throw new FileNotFoundException("File not found", path);
			return new StreamWriter(new MockFileStream(accessor, FullName, true));
			//return ((MockFileDataModifier) MockFileData).AppendText();
		}

		public override FileInfoBase CopyTo(string destFileName)
		{
			new MockFile(accessor).Copy(FullName, destFileName);
			return accessor.FileInfo.FromFileName(destFileName);
		}

		public override FileInfoBase CopyTo(string destFileName, bool overwrite)
		{
			new MockFile(accessor).Copy(FullName, destFileName, overwrite);
			return accessor.FileInfo.FromFileName(destFileName);
		}

		public override Stream Create() => new MockFile(accessor).Create(FullName);

		public override StreamWriter CreateText() => new MockFile(accessor).CreateText(FullName);

		public override void Decrypt()
		{
			if (fileElement == null) throw new FileNotFoundException("File not found", path);
			var contents = fileElement.ToArray();
			for (var i = 0; i < contents.Length; i++)
				contents[i] ^= (byte)(i % 256);
			fileElement.Assign( contents );
		}

		public override void Encrypt()
		{
			if (fileElement == null) throw new FileNotFoundException("File not found", path);
			var contents = fileElement.ToArray();
			for(var i = 0; i < contents.Length; i++)
				contents[i] ^= (byte) (i % 256);
			fileElement.Assign( contents );
		}

		public override FileSecurity GetAccessControl()
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override FileSecurity GetAccessControl(AccessControlSections includeSections)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override void MoveTo(string destFileName)
		{
			var movedFileInfo = CopyTo(destFileName);
			Delete();
			path = movedFileInfo.FullName;
		}

		public override Stream Open(FileMode mode) => new MockFile(accessor).Open(FullName, mode);

		public override Stream Open(FileMode mode, FileAccess access) => new MockFile(accessor).Open(FullName, mode, access);

		public override Stream Open(FileMode mode, FileAccess access, FileShare share) => new MockFile(accessor).Open(FullName, mode, access, share);

		public override Stream OpenRead() => new MockFileStream(accessor, path);

		public override StreamReader OpenText() => new StreamReader(OpenRead());

		public override Stream OpenWrite() => new MockFileStream(accessor, path);

		public override FileInfoBase Replace(string destinationFileName, string destinationBackupFileName)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override FileInfoBase Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override void SetAccessControl(FileSecurity fileSecurity)
		{
			throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
		}

		public override DirectoryInfoBase Directory => accessor.DirectoryInfo.FromDirectoryName(DirectoryName);

		// System.IO.Path.GetDirectoryName does only string manipulation,
		// so it's safe to delegate.
		public override string DirectoryName => Path.GetDirectoryName(path);

		public override bool IsReadOnly
		{
			get
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				return (fileElement.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
			}
			set
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				if(value)
					fileElement.Attributes |= FileAttributes.ReadOnly;
				else
					fileElement.Attributes &= ~FileAttributes.ReadOnly;
			}
		}

		public override long Length
		{
			get
			{
				if (fileElement == null) throw new FileNotFoundException("File not found", path);
				return fileElement.ToArray().LongLength;
			}
		}
	}
}