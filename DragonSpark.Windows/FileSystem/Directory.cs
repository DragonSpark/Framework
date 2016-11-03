using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace DragonSpark.Windows.FileSystem
{
	public class Directory : SingletonScope<DirectoryBase>, IDirectory
	{
		public static Directory Default { get; } = new Directory();
		Directory() : base( () => new DirectoryWrapper() ) {}
		
		readonly Func<DirectoryInfoBase, IDirectoryInfo> directorySource = Defaults.Directory;

		public Directory( DirectoryBase source ) : this( source, Defaults.Directory ) {}

		[UsedImplicitly]
		public Directory( DirectoryBase source, Func<DirectoryInfoBase, IDirectoryInfo> directorySource ) : base( source )
		{
			this.directorySource = directorySource;
		}

		public IDirectoryInfo CreateDirectory( string path ) => directorySource( Get().CreateDirectory( path ) );
		public IDirectoryInfo CreateDirectory( string path, DirectorySecurity directorySecurity ) => directorySource( Get().CreateDirectory( path, directorySecurity ) );
		public void Delete( string path ) => Get().Delete( path );
		public void Delete( string path, bool recursive ) => Get().Delete( path, recursive );
		public bool Exists( string path ) => Get().Exists( path );
		public DirectorySecurity GetAccessControl( string path ) => Get().GetAccessControl( path );
		public DirectorySecurity GetAccessControl( string path, AccessControlSections includeSections ) => Get().GetAccessControl( path, includeSections );
		public DateTime GetCreationTime( string path ) => Get().GetCreationTime( path );
		public DateTime GetCreationTimeUtc( string path ) => Get().GetCreationTimeUtc( path );
		public string GetCurrentDirectory() => Get().GetCurrentDirectory();
		public string[] GetDirectories( string path ) => Get().GetDirectories( path );
		public string[] GetDirectories( string path, string searchPattern ) => Get().GetDirectories( path, searchPattern );
		public string[] GetDirectories( string path, string searchPattern, SearchOption searchOption ) => Get().GetDirectories( path, searchPattern, searchOption );
		public string GetDirectoryRoot( string path ) => Get().GetDirectoryRoot( path );
		public string[] GetFiles( string path ) => Get().GetFiles( path );
		public string[] GetFiles( string path, string searchPattern ) => Get().GetFiles( path, searchPattern );
		public string[] GetFiles( string path, string searchPattern, SearchOption searchOption ) => Get().GetFiles( path, searchPattern, searchOption );
		public string[] GetFileSystemEntries( string path ) => Get().GetFileSystemEntries( path );
		public string[] GetFileSystemEntries( string path, string searchPattern ) => Get().GetFileSystemEntries( path, searchPattern );
		public DateTime GetLastAccessTime( string path ) => Get().GetLastAccessTime( path );
		public DateTime GetLastAccessTimeUtc( string path ) => Get().GetLastAccessTimeUtc( path );
		public DateTime GetLastWriteTime( string path ) => Get().GetLastWriteTime( path );
		public DateTime GetLastWriteTimeUtc( string path ) => Get().GetLastWriteTimeUtc( path );
		public string[] GetLogicalDrives() => Get().GetLogicalDrives();
		public IDirectoryInfo GetParent( string path )
		{
			var parent = Get().GetParent( path );
			var result = parent != null ? directorySource( parent ) : null;
			return result;
		}
		public void Move( string sourceDirName, string destDirName ) => Get().Move( sourceDirName, destDirName );
		public void SetAccessControl( string path, DirectorySecurity directorySecurity ) => Get().SetAccessControl( path, directorySecurity );
		public void SetCreationTime( string path, DateTime creationTime ) => Get().SetCreationTime( path, creationTime );
		public void SetCreationTimeUtc( string path, DateTime creationTimeUtc ) => Get().SetCreationTimeUtc( path, creationTimeUtc );
		public void SetCurrentDirectory( string path ) => Get().SetCurrentDirectory( path );
		public void SetLastAccessTime( string path, DateTime lastAccessTime ) => Get().SetLastAccessTime( path, lastAccessTime );
		public void SetLastAccessTimeUtc( string path, DateTime lastAccessTimeUtc ) => Get().SetLastAccessTimeUtc( path, lastAccessTimeUtc );
		public void SetLastWriteTime( string path, DateTime lastWriteTime ) => Get().SetLastWriteTime( path, lastWriteTime );
		public void SetLastWriteTimeUtc( string path, DateTime lastWriteTimeUtc ) => Get().SetLastWriteTimeUtc( path, lastWriteTimeUtc );
		public IEnumerable<string> EnumerateDirectories( string path ) => Get().EnumerateDirectories( path );
		public IEnumerable<string> EnumerateDirectories( string path, string searchPattern ) => Get().EnumerateDirectories( path, searchPattern );
		public IEnumerable<string> EnumerateDirectories( string path, string searchPattern, SearchOption searchOption ) => Get().EnumerateDirectories( path, searchPattern, searchOption );
		public IEnumerable<string> EnumerateFiles( string path ) => Get().EnumerateFiles( path );
		public IEnumerable<string> EnumerateFiles( string path, string searchPattern ) => Get().EnumerateFiles( path, searchPattern );
		public IEnumerable<string> EnumerateFiles( string path, string searchPattern, SearchOption searchOption ) => Get().EnumerateFiles( path, searchPattern, searchOption );
		public IEnumerable<string> EnumerateFileSystemEntries( string path ) => Get().EnumerateFileSystemEntries( path );
		public IEnumerable<string> EnumerateFileSystemEntries( string path, string searchPattern ) => Get().EnumerateFileSystemEntries( path, searchPattern );
		public IEnumerable<string> EnumerateFileSystemEntries( string path, string searchPattern, SearchOption searchOption ) => Get().EnumerateFileSystemEntries( path, searchPattern, searchOption );
	}
}