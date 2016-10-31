using DragonSpark.Sources;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace DragonSpark.Windows.FileSystem
{
	public class Directory : IDirectory
	{
		public static ISource<IDirectory> Current { get; } = new Scope<IDirectory>( Sources.Factory.GlobalCache( () => new Directory() ) );

		public static Directory Default { get; } = new Directory();
		Directory() : this( DefaultImplementation.Implementation.Get() ) {}

		readonly DirectoryBase source;
		readonly Func<DirectoryInfoBase, IDirectoryInfo> directorySource;

		public Directory( DirectoryBase source ) : this( source, Defaults.Directory ) {}

		[UsedImplicitly]
		public Directory( DirectoryBase source, Func<DirectoryInfoBase, IDirectoryInfo> directorySource )
		{
			this.source = source;
			this.directorySource = directorySource;
		}

		public IDirectoryInfo CreateDirectory( string path ) => directorySource( source.CreateDirectory( path ) );

		public IDirectoryInfo CreateDirectory( string path, DirectorySecurity directorySecurity ) => directorySource( source.CreateDirectory( path, directorySecurity ) );

		public void Delete( string path ) => source.Delete( path );

		public void Delete( string path, bool recursive ) => source.Delete( path, recursive );

		public bool Exists( string path ) => source.Exists( path );

		public DirectorySecurity GetAccessControl( string path ) => source.GetAccessControl( path );

		public DirectorySecurity GetAccessControl( string path, AccessControlSections includeSections ) => source.GetAccessControl( path, includeSections );

		public DateTime GetCreationTime( string path ) => source.GetCreationTime( path );

		public DateTime GetCreationTimeUtc( string path ) => source.GetCreationTimeUtc( path );

		public string GetCurrentDirectory() => source.GetCurrentDirectory();

		public string[] GetDirectories( string path ) => source.GetDirectories( path );

		public string[] GetDirectories( string path, string searchPattern ) => source.GetDirectories( path, searchPattern );

		public string[] GetDirectories( string path, string searchPattern, SearchOption searchOption ) => source.GetDirectories( path, searchPattern, searchOption );

		public string GetDirectoryRoot( string path ) => source.GetDirectoryRoot( path );

		public string[] GetFiles( string path ) => source.GetFiles( path );

		public string[] GetFiles( string path, string searchPattern ) => source.GetFiles( path, searchPattern );

		public string[] GetFiles( string path, string searchPattern, SearchOption searchOption ) => source.GetFiles( path, searchPattern, searchOption );

		public string[] GetFileSystemEntries( string path ) => source.GetFileSystemEntries( path );

		public string[] GetFileSystemEntries( string path, string searchPattern ) => source.GetFileSystemEntries( path, searchPattern );

		public DateTime GetLastAccessTime( string path ) => source.GetLastAccessTime( path );

		public DateTime GetLastAccessTimeUtc( string path ) => source.GetLastAccessTimeUtc( path );

		public DateTime GetLastWriteTime( string path ) => source.GetLastWriteTime( path );

		public DateTime GetLastWriteTimeUtc( string path ) => source.GetLastWriteTimeUtc( path );

		public string[] GetLogicalDrives() => source.GetLogicalDrives();

		public IDirectoryInfo GetParent( string path )
		{
			var parent = source.GetParent( path );
			var result = parent != null ? directorySource( parent ) : null;
			return result;
		}

		public void Move( string sourceDirName, string destDirName ) => source.Move( sourceDirName, destDirName );

		public void SetAccessControl( string path, DirectorySecurity directorySecurity ) => source.SetAccessControl( path, directorySecurity );

		public void SetCreationTime( string path, DateTime creationTime ) => source.SetCreationTime( path, creationTime );

		public void SetCreationTimeUtc( string path, DateTime creationTimeUtc ) => source.SetCreationTimeUtc( path, creationTimeUtc );

		public void SetCurrentDirectory( string path ) => source.SetCurrentDirectory( path );

		public void SetLastAccessTime( string path, DateTime lastAccessTime ) => source.SetLastAccessTime( path, lastAccessTime );

		public void SetLastAccessTimeUtc( string path, DateTime lastAccessTimeUtc ) => source.SetLastAccessTimeUtc( path, lastAccessTimeUtc );

		public void SetLastWriteTime( string path, DateTime lastWriteTime ) => source.SetLastWriteTime( path, lastWriteTime );

		public void SetLastWriteTimeUtc( string path, DateTime lastWriteTimeUtc ) => source.SetLastWriteTimeUtc( path, lastWriteTimeUtc );

		public IEnumerable<string> EnumerateDirectories( string path ) => source.EnumerateDirectories( path );

		public IEnumerable<string> EnumerateDirectories( string path, string searchPattern ) => source.EnumerateDirectories( path, searchPattern );

		public IEnumerable<string> EnumerateDirectories( string path, string searchPattern, SearchOption searchOption ) => source.EnumerateDirectories( path, searchPattern, searchOption );

		public IEnumerable<string> EnumerateFiles( string path ) => source.EnumerateFiles( path );

		public IEnumerable<string> EnumerateFiles( string path, string searchPattern ) => source.EnumerateFiles( path, searchPattern );

		public IEnumerable<string> EnumerateFiles( string path, string searchPattern, SearchOption searchOption ) => source.EnumerateFiles( path, searchPattern, searchOption );

		public IEnumerable<string> EnumerateFileSystemEntries( string path ) => source.EnumerateFileSystemEntries( path );

		public IEnumerable<string> EnumerateFileSystemEntries( string path, string searchPattern ) => source.EnumerateFileSystemEntries( path, searchPattern );

		public IEnumerable<string> EnumerateFileSystemEntries( string path, string searchPattern, SearchOption searchOption ) => source.EnumerateFileSystemEntries( path, searchPattern, searchOption );

		public sealed class DefaultImplementation : Scope<DirectoryBase>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( Sources.Factory.GlobalCache( () => new DirectoryWrapper() ) ) {}
		}
	}
}