using DragonSpark.Sources;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using System.Text;

namespace DragonSpark.Windows.FileSystem
{
	public class File : IFile
	{
		public static IScope<IFile> Current { get; } = new Scope<IFile>( Sources.Factory.GlobalCache( () => new File() ) );

		public static File Default { get; } = new File();
		File() : this( DefaultImplementation.Implementation.Get() ) {}

		readonly FileBase source;

		public File( FileBase source )
		{
			this.source = source;
		}

		public void AppendAllLines( string path, IEnumerable<string> contents ) => source.AppendAllLines( path, contents );

		public void AppendAllLines( string path, IEnumerable<string> contents, Encoding encoding ) => source.AppendAllLines( path, contents, encoding );

		public void AppendAllText( string path, string contents ) => source.AppendAllText( path, contents );

		public void AppendAllText( string path, string contents, Encoding encoding ) => source.AppendAllText( path, contents, encoding );

		public StreamWriter AppendText( string path ) => source.AppendText( path );

		public void Copy( string sourceFileName, string destFileName ) => source.Copy( sourceFileName, destFileName );

		public void Copy( string sourceFileName, string destFileName, bool overwrite ) => source.Copy( sourceFileName, destFileName, overwrite );

		public Stream Create( string path ) => source.Create( path );

		public Stream Create( string path, int bufferSize ) => source.Create( path, bufferSize );

		public Stream Create( string path, int bufferSize, FileOptions options ) => source.Create( path, bufferSize, options );

		public Stream Create( string path, int bufferSize, FileOptions options, FileSecurity fileSecurity ) => source.Create( path, bufferSize, options, fileSecurity );

		public StreamWriter CreateText( string path ) => source.CreateText( path );

		public void Decrypt( string path ) => source.Decrypt( path );

		public void Delete( string path ) => source.Delete( path );

		public void Encrypt( string path ) => source.Encrypt( path );

		public bool Exists( string path ) => source.Exists( path );

		public FileSecurity GetAccessControl( string path ) => source.GetAccessControl( path );

		public FileSecurity GetAccessControl( string path, AccessControlSections includeSections ) => source.GetAccessControl( path, includeSections );

		public FileAttributes GetAttributes( string path ) => source.GetAttributes( path );

		public DateTime GetCreationTime( string path ) => source.GetCreationTime( path );

		public DateTime GetCreationTimeUtc( string path ) => source.GetCreationTimeUtc( path );

		public DateTime GetLastAccessTime( string path ) => source.GetLastAccessTime( path );

		public DateTime GetLastAccessTimeUtc( string path ) => source.GetLastAccessTimeUtc( path );

		public DateTime GetLastWriteTime( string path ) => source.GetLastWriteTime( path );

		public DateTime GetLastWriteTimeUtc( string path ) => source.GetLastWriteTimeUtc( path );

		public void Move( string sourceFileName, string destFileName ) => source.Move( sourceFileName, destFileName );

		public Stream Open( string path, FileMode mode ) => source.Open( path, mode );

		public Stream Open( string path, FileMode mode, FileAccess access ) => source.Open( path, mode, access );

		public Stream Open( string path, FileMode mode, FileAccess access, FileShare share ) => source.Open( path, mode, access, share );

		public Stream OpenRead( string path ) => source.OpenRead( path );

		public StreamReader OpenText( string path ) => source.OpenText( path );

		public Stream OpenWrite( string path ) => source.OpenWrite( path );

		public byte[] ReadAllBytes( string path ) => source.ReadAllBytes( path );

		public string[] ReadAllLines( string path ) => source.ReadAllLines( path );

		public string[] ReadAllLines( string path, Encoding encoding ) => source.ReadAllLines( path, encoding );

		public string ReadAllText( string path ) => source.ReadAllText( path );

		public string ReadAllText( string path, Encoding encoding ) => source.ReadAllText( path, encoding );

		public IEnumerable<string> ReadLines( string path ) => source.ReadLines( path );

		public IEnumerable<string> ReadLines( string path, Encoding encoding ) => source.ReadLines( path, encoding );

		public void Replace( string sourceFileName, string destinationFileName, string destinationBackupFileName ) => source.Replace( sourceFileName, destinationFileName, destinationBackupFileName );

		public void Replace( string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors ) => source.Replace( sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors );

		public void SetAccessControl( string path, FileSecurity fileSecurity ) => source.SetAccessControl( path, fileSecurity );

		public void SetAttributes( string path, FileAttributes fileAttributes ) => source.SetAttributes( path, fileAttributes );

		public void SetCreationTime( string path, DateTime creationTime ) => source.SetCreationTime( path, creationTime );

		public void SetCreationTimeUtc( string path, DateTime creationTimeUtc ) => source.SetCreationTimeUtc( path, creationTimeUtc );

		public void SetLastAccessTime( string path, DateTime lastAccessTime ) => source.SetLastAccessTime( path, lastAccessTime );

		public void SetLastAccessTimeUtc( string path, DateTime lastAccessTimeUtc ) => source.SetLastAccessTimeUtc( path, lastAccessTimeUtc );

		public void SetLastWriteTime( string path, DateTime lastWriteTime ) => source.SetLastWriteTime( path, lastWriteTime );

		public void SetLastWriteTimeUtc( string path, DateTime lastWriteTimeUtc ) => source.SetLastWriteTimeUtc( path, lastWriteTimeUtc );

		public void WriteAllBytes( string path, byte[] bytes ) => source.WriteAllBytes( path, bytes );

		public void WriteAllLines( string path, IEnumerable<string> contents ) => source.WriteAllLines( path, contents );

		public void WriteAllLines( string path, IEnumerable<string> contents, Encoding encoding ) => source.WriteAllLines( path, contents, encoding );

		public void WriteAllLines( string path, string[] contents ) => source.WriteAllLines( path, contents );

		public void WriteAllLines( string path, string[] contents, Encoding encoding ) => source.WriteAllLines( path, contents, encoding );

		public void WriteAllText( string path, string contents ) => source.WriteAllText( path, contents );

		public void WriteAllText( string path, string contents, Encoding encoding ) => source.WriteAllText( path, contents, encoding );

		public sealed class DefaultImplementation : Scope<FileBase>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( Sources.Factory.GlobalCache( () => new FileWrapper() ) ) {}
		}
	}
}