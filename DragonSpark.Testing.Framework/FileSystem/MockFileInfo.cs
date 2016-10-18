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
		readonly IFileSystem fileSystem;
		string path;
		readonly IFileElement fileElement;
		readonly MockFile file;
		readonly MockPath mockPath;

		public MockFileInfo(IFileSystem fileSystem, string path)
		{
			this.fileSystem = fileSystem;
			this.path = path;
			fileElement = fileSystem.GetFile( path );
			if (fileElement == null) throw new FileNotFoundException("File not found", path);
			file = new MockFile(this.fileSystem);
			mockPath = new MockPath(this.fileSystem);
		}

		public override void Delete() => fileSystem.RemoveFile(path);

		public override void Refresh() {}

		public override FileAttributes Attributes
		{
			get { return fileElement.Attributes; }
			set { fileElement.Attributes = value; }
		}

		public override DateTime CreationTime
		{
			get { return fileElement.CreationTime.DateTime; }
			set { fileElement.CreationTime = value; }
		}

		public override DateTime CreationTimeUtc
		{
			get { return fileElement.CreationTime.UtcDateTime; }
			set { fileElement.CreationTime = value.ToLocalTime(); }
		}

		public override bool Exists => fileElement != null;

		// System.IO.Path.GetExtension does only string manipulation,
		// so it's safe to delegate.
		public override string Extension => Path.GetExtension(path);

		public override string FullName => path;

		public override DateTime LastAccessTime
		{
			get { return fileElement.LastAccessTime.DateTime; }
			set { fileElement.LastAccessTime = value; }
		}

		public override DateTime LastAccessTimeUtc
		{
			get { return fileElement.LastAccessTime.UtcDateTime; }
			set { fileElement.LastAccessTime = value; }
		}

		public override DateTime LastWriteTime
		{
			get { return fileElement.LastWriteTime.DateTime; }
			set { fileElement.LastWriteTime = value; }
		}

		public override DateTime LastWriteTimeUtc
		{
			get { return fileElement.LastWriteTime.UtcDateTime; }
			set { fileElement.LastWriteTime = value.ToLocalTime(); }
		}

		public override string Name => mockPath.GetFileName(path);

		public override StreamWriter AppendText() => new StreamWriter(new MockFileStream(fileSystem, FullName, true));

		public override FileInfoBase CopyTo(string destFileName)
		{
			file.Copy(FullName, destFileName);
			return fileSystem.FromFileName(destFileName);
		}

		public override FileInfoBase CopyTo(string destFileName, bool overwrite)
		{
			file.Copy(FullName, destFileName, overwrite);
			return fileSystem.FromFileName(destFileName);
		}

		public override Stream Create() => file.Create(FullName);

		public override StreamWriter CreateText() => file.CreateText(FullName);

		public override void Decrypt()
		{
			var contents = fileElement.ToArray();
			for (var i = 0; i < contents.Length; i++)
				contents[i] ^= (byte)(i % 256);
			fileElement.Assign( contents );
		}

		public override void Encrypt()
		{
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

		public override Stream Open(FileMode mode) => file.Open(FullName, mode);

		public override Stream Open(FileMode mode, FileAccess access) => file.Open(FullName, mode, access);

		public override Stream Open(FileMode mode, FileAccess access, FileShare share) => file.Open(FullName, mode, access, share);

		public override Stream OpenRead() => new MockFileStream(fileSystem, path);

		public override StreamReader OpenText() => new StreamReader(OpenRead());

		public override Stream OpenWrite() => new MockFileStream(fileSystem, path);

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

		public override DirectoryInfoBase Directory => fileSystem.FromDirectoryName(DirectoryName);

		// System.IO.Path.GetDirectoryName does only string manipulation,
		// so it's safe to delegate.
		public override string DirectoryName => Path.GetDirectoryName(path);

		public override bool IsReadOnly
		{
			get { return ( fileElement.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly; }
			set
			{
				if(value)
					fileElement.Attributes |= FileAttributes.ReadOnly;
				else
					fileElement.Attributes &= ~FileAttributes.ReadOnly;
			}
		}

		public override long Length => fileElement.ToArray().LongLength;
	}
}