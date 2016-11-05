using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace DragonSpark.Windows.FileSystem
{
	public interface IFileInfo : IFileSystemInfo
	{
		IDirectoryInfo Directory { get; }
		string DirectoryName { get; }
		bool IsReadOnly { get; set; }
		long Length { get; }

		StreamWriter AppendText();
		IFileInfo CopyTo( string destFileName );
		IFileInfo CopyTo( string destFileName, bool overwrite );
		Stream Create();
		StreamWriter CreateText();
		void Decrypt();
		void Encrypt();
		FileSecurity GetAccessControl();
		FileSecurity GetAccessControl( AccessControlSections includeSections );
		void MoveTo( string destFileName );
		Stream Open( FileMode mode );
		Stream Open( FileMode mode, FileAccess access );
		Stream Open( FileMode mode, FileAccess access, FileShare share );
		Stream OpenRead();
		StreamReader OpenText();
		Stream OpenWrite();
		IFileInfo Replace( string destinationFileName, string destinationBackupFileName );
		IFileInfo Replace( string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors );
		void SetAccessControl( FileSecurity fileSecurity );
	}

	public class FileInfo : FileSystemInfoBase<FileInfoBase>, IFileInfo
	{
		readonly Func<FileInfoBase, IFileInfo> fileSource;

		public FileInfo( FileInfoBase source ) : this( source, Defaults.Directory, Defaults.File ) {}
		public FileInfo( FileInfoBase source, Func<DirectoryInfoBase, IDirectoryInfo> directorySource, Func<FileInfoBase, IFileInfo> fileSource ) : base( source )
		{
			this.fileSource = fileSource;
			Directory = directorySource( source.Directory );
		}

		public StreamWriter AppendText() => Source.AppendText();

		public IFileInfo CopyTo( string destFileName ) => fileSource( Source.CopyTo( destFileName ) );

		public IFileInfo CopyTo( string destFileName, bool overwrite ) => fileSource( Source.CopyTo( destFileName, overwrite ) );

		public Stream Create() => Source.Create();

		public StreamWriter CreateText() => Source.CreateText();

		public void Decrypt() => Source.Decrypt();

		public void Encrypt() => Source.Encrypt();

		public FileSecurity GetAccessControl() => Source.GetAccessControl();

		public FileSecurity GetAccessControl( AccessControlSections includeSections ) => Source.GetAccessControl( includeSections );

		public void MoveTo( string destFileName ) => Source.MoveTo( destFileName );

		public Stream Open( FileMode mode ) => Source.Open( mode );

		public Stream Open( FileMode mode, FileAccess access ) => Source.Open( mode, access );

		public Stream Open( FileMode mode, FileAccess access, FileShare share ) => Source.Open( mode, access, share );

		public Stream OpenRead() => Source.OpenRead();

		public StreamReader OpenText() => Source.OpenText();

		public Stream OpenWrite() => Source.OpenWrite();

		public IFileInfo Replace( string destinationFileName, string destinationBackupFileName ) => fileSource( Source.Replace( destinationFileName, destinationBackupFileName ) );

		public IFileInfo Replace( string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors ) => fileSource( Source.Replace( destinationFileName, destinationBackupFileName, ignoreMetadataErrors ) );

		public void SetAccessControl( FileSecurity fileSecurity ) => Source.SetAccessControl( fileSecurity );

		public IDirectoryInfo Directory { get; }

		public string DirectoryName => Source.DirectoryName;

		public bool IsReadOnly
		{
			get { return Source.IsReadOnly; }
			set { Source.IsReadOnly = value; }
		}

		public long Length => Source.Length;
	}
}