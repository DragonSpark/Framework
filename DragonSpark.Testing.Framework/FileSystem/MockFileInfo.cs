using DragonSpark.Sources;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using DragonSpark.Sources.Scopes;
using File = DragonSpark.Windows.FileSystem.File;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockFileInfo : FileInfoBase
	{
		readonly IFileSystemRepository repository;
		readonly IFile file;
		readonly IPath path;
		readonly IElementSource<IFileElement> element;

		public MockFileInfo( string filePath ) : this( FileSystemRepository.Default, filePath ) {}

		public MockFileInfo( IFileSystemRepository repository, string filePath ) : this( repository, Windows.FileSystem.Path.Default, File.Default, filePath ) {}

		[UsedImplicitly]
		public MockFileInfo( IFileSystemRepository repository, IPath path, IFile file, string filePath ) : this( repository, path, file, new ElementSource<IFileElement>( repository, filePath ) ) {}

		MockFileInfo( IFileSystemRepository repository, IPath path, IFile file, IElementSource<IFileElement> element )
		{
			this.repository = repository;
			this.path = path;
			this.file = file;
			this.element = element;
		}

		IFileElement Element => element.Get();

		public override void Delete()
		{
			repository.Remove( FullName );
			Refresh();
		}

		public override void Refresh() => element.Assign( repository.GetFile( FullName ) );

		public override FileAttributes Attributes
		{
			get { return Element.Attributes; }
			set { Element.Attributes = value; }
		}

		public override DateTime CreationTime
		{
			get { return Element.CreationTime.DateTime; }
			set { Element.CreationTime = value; }
		}

		public override DateTime CreationTimeUtc
		{
			get { return Element.CreationTime.UtcDateTime; }
			set { Element.CreationTime = value.ToLocalTime(); }
		}

		public override bool Exists => repository.Get( FullName ) is IFileElement;

		public override string Extension => path.GetExtension( FullName );

		public override string FullName => element.Path;

		public override DateTime LastAccessTime
		{
			get { return Element.LastAccessTime.DateTime; }
			set { Element.LastAccessTime = value; }
		}

		public override DateTime LastAccessTimeUtc
		{
			get { return Element.LastAccessTime.UtcDateTime; }
			set { Element.LastAccessTime = value; }
		}

		public override DateTime LastWriteTime
		{
			get { return Element.LastWriteTime.DateTime; }
			set { Element.LastWriteTime = value; }
		}

		public override DateTime LastWriteTimeUtc
		{
			get { return Element.LastWriteTime.UtcDateTime; }
			set { Element.LastWriteTime = value.ToLocalTime(); }
		}

		public override string Name => path.GetFileName( FullName );

		public override StreamWriter AppendText() => new StreamWriter( new MockFileStream( repository, FullName, true ) );

		public override FileInfoBase CopyTo( string destFileName )
		{
			file.Copy( FullName, destFileName );
			return repository.FromFileName( destFileName );
		}

		public override FileInfoBase CopyTo( string destFileName, bool overwrite )
		{
			file.Copy( FullName, destFileName, overwrite );
			return repository.FromFileName( destFileName );
		}

		public override Stream Create() => file.Create( FullName );

		public override StreamWriter CreateText() => file.CreateText( FullName );

		public override void Decrypt()
		{
			var contents = Element.Unwrap();
			for ( var i = 0; i < contents.Length; i++ )
				contents[i] ^= (byte)( i % 256 );
			Element.Assign( contents );
		}

		public override void Encrypt()
		{
			var contents = Element.Unwrap();
			for ( var i = 0; i < contents.Length; i++ )
				contents[i] ^= (byte)( i % 256 );
			Element.Assign( contents );
		}

		public override FileSecurity GetAccessControl()
		{
			throw new NotImplementedException( Properties.Resources.NOT_IMPLEMENTED_EXCEPTION );
		}

		public override FileSecurity GetAccessControl( AccessControlSections includeSections )
		{
			throw new NotImplementedException( Properties.Resources.NOT_IMPLEMENTED_EXCEPTION );
		}

		public override void MoveTo( string destFileName )
		{
			var movedFileInfo = CopyTo( destFileName );
			Delete();

			element.Assign( repository.GetFile( movedFileInfo.FullName ) );
		}

		public override Stream Open( FileMode mode ) => file.Open( FullName, mode );

		public override Stream Open( FileMode mode, FileAccess access ) => file.Open( FullName, mode, access );

		public override Stream Open( FileMode mode, FileAccess access, FileShare share ) => file.Open( FullName, mode, access, share );

		public override Stream OpenRead() => new MockFileStream( repository, FullName );

		public override StreamReader OpenText() => new StreamReader( OpenRead() );

		public override Stream OpenWrite() => new MockFileStream( repository, FullName );

		public override FileInfoBase Replace( string destinationFileName, string destinationBackupFileName )
		{
			throw new NotImplementedException( Properties.Resources.NOT_IMPLEMENTED_EXCEPTION );
		}

		public override FileInfoBase Replace( string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors )
		{
			throw new NotImplementedException( Properties.Resources.NOT_IMPLEMENTED_EXCEPTION );
		}

		public override void SetAccessControl( FileSecurity fileSecurity )
		{
			throw new NotImplementedException( Properties.Resources.NOT_IMPLEMENTED_EXCEPTION );
		}

		public override DirectoryInfoBase Directory => repository.FromDirectoryName( DirectoryName );

		public override string DirectoryName => path.GetDirectoryName( FullName );

		public override bool IsReadOnly
		{
			get { return ( Element.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly; }
			set
			{
				if ( value )
					Element.Attributes |= FileAttributes.ReadOnly;
				else
					Element.Attributes &= ~FileAttributes.ReadOnly;
			}
		}

		public override long Length => Element.Unwrap().LongLength;
	}
}