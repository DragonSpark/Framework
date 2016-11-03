using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using System.Text;

namespace DragonSpark.Windows.FileSystem
{
	public class File : SingletonScope<FileBase>, IFile
	{
		public static File Default { get; } = new File();
		File() : base( () => new FileWrapper() ) {}

		[UsedImplicitly]
		public File( FileBase source ) :  base( source ) {}

		public void AppendAllLines( string path, IEnumerable<string> contents ) => Get().AppendAllLines( path, contents );
		public void AppendAllLines( string path, IEnumerable<string> contents, Encoding encoding ) => Get().AppendAllLines( path, contents, encoding );
		public void AppendAllText( string path, string contents ) => Get().AppendAllText( path, contents );
		public void AppendAllText( string path, string contents, Encoding encoding ) => Get().AppendAllText( path, contents, encoding );
		public StreamWriter AppendText( string path ) => Get().AppendText( path );
		public void Copy( string sourceFileName, string destFileName ) => Get().Copy( sourceFileName, destFileName );
		public void Copy( string sourceFileName, string destFileName, bool overwrite ) => Get().Copy( sourceFileName, destFileName, overwrite );
		public Stream Create( string path ) => Get().Create( path );
		public Stream Create( string path, int bufferSize ) => Get().Create( path, bufferSize );
		public Stream Create( string path, int bufferSize, FileOptions options ) => Get().Create( path, bufferSize, options );
		public Stream Create( string path, int bufferSize, FileOptions options, FileSecurity fileSecurity ) => Get().Create( path, bufferSize, options, fileSecurity );
		public StreamWriter CreateText( string path ) => Get().CreateText( path );
		public void Decrypt( string path ) => Get().Decrypt( path );
		public void Delete( string path ) => Get().Delete( path );
		public void Encrypt( string path ) => Get().Encrypt( path );
		public bool Exists( string path ) => Get().Exists( path );
		public FileSecurity GetAccessControl( string path ) => Get().GetAccessControl( path );
		public FileSecurity GetAccessControl( string path, AccessControlSections includeSections ) => Get().GetAccessControl( path, includeSections );
		public FileAttributes GetAttributes( string path ) => Get().GetAttributes( path );
		public DateTime GetCreationTime( string path ) => Get().GetCreationTime( path );
		public DateTime GetCreationTimeUtc( string path ) => Get().GetCreationTimeUtc( path );
		public DateTime GetLastAccessTime( string path ) => Get().GetLastAccessTime( path );
		public DateTime GetLastAccessTimeUtc( string path ) => Get().GetLastAccessTimeUtc( path );
		public DateTime GetLastWriteTime( string path ) => Get().GetLastWriteTime( path );
		public DateTime GetLastWriteTimeUtc( string path ) => Get().GetLastWriteTimeUtc( path );
		public void Move( string sourceFileName, string destFileName ) => Get().Move( sourceFileName, destFileName );
		public Stream Open( string path, FileMode mode ) => Get().Open( path, mode );
		public Stream Open( string path, FileMode mode, FileAccess access ) => Get().Open( path, mode, access );
		public Stream Open( string path, FileMode mode, FileAccess access, FileShare share ) => Get().Open( path, mode, access, share );
		public Stream OpenRead( string path ) => Get().OpenRead( path );
		public StreamReader OpenText( string path ) => Get().OpenText( path );
		public Stream OpenWrite( string path ) => Get().OpenWrite( path );
		public byte[] ReadAllBytes( string path ) => Get().ReadAllBytes( path );
		public string[] ReadAllLines( string path ) => Get().ReadAllLines( path );
		public string[] ReadAllLines( string path, Encoding encoding ) => Get().ReadAllLines( path, encoding );
		public string ReadAllText( string path ) => Get().ReadAllText( path );
		public string ReadAllText( string path, Encoding encoding ) => Get().ReadAllText( path, encoding );
		public IEnumerable<string> ReadLines( string path ) => Get().ReadLines( path );
		public IEnumerable<string> ReadLines( string path, Encoding encoding ) => Get().ReadLines( path, encoding );
		public void Replace( string sourceFileName, string destinationFileName, string destinationBackupFileName ) => Get().Replace( sourceFileName, destinationFileName, destinationBackupFileName );
		public void Replace( string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors ) => Get().Replace( sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors );
		public void SetAccessControl( string path, FileSecurity fileSecurity ) => Get().SetAccessControl( path, fileSecurity );
		public void SetAttributes( string path, FileAttributes fileAttributes ) => Get().SetAttributes( path, fileAttributes );
		public void SetCreationTime( string path, DateTime creationTime ) => Get().SetCreationTime( path, creationTime );
		public void SetCreationTimeUtc( string path, DateTime creationTimeUtc ) => Get().SetCreationTimeUtc( path, creationTimeUtc );
		public void SetLastAccessTime( string path, DateTime lastAccessTime ) => Get().SetLastAccessTime( path, lastAccessTime );
		public void SetLastAccessTimeUtc( string path, DateTime lastAccessTimeUtc ) => Get().SetLastAccessTimeUtc( path, lastAccessTimeUtc );
		public void SetLastWriteTime( string path, DateTime lastWriteTime ) => Get().SetLastWriteTime( path, lastWriteTime );
		public void SetLastWriteTimeUtc( string path, DateTime lastWriteTimeUtc ) => Get().SetLastWriteTimeUtc( path, lastWriteTimeUtc );
		public void WriteAllBytes( string path, byte[] bytes ) => Get().WriteAllBytes( path, bytes );
		public void WriteAllLines( string path, IEnumerable<string> contents ) => Get().WriteAllLines( path, contents );
		public void WriteAllLines( string path, IEnumerable<string> contents, Encoding encoding ) => Get().WriteAllLines( path, contents, encoding );
		public void WriteAllLines( string path, string[] contents ) => Get().WriteAllLines( path, contents );
		public void WriteAllLines( string path, string[] contents, Encoding encoding ) => Get().WriteAllLines( path, contents, encoding );
		public void WriteAllText( string path, string contents ) => Get().WriteAllText( path, contents );
		public void WriteAllText( string path, string contents, Encoding encoding ) => Get().WriteAllText( path, contents, encoding );
	}
}