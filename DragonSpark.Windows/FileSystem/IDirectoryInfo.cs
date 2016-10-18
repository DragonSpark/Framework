using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;

namespace DragonSpark.Windows.FileSystem
{
	public interface IDirectoryInfo : IFileSystemInfo
	{
		IDirectoryInfo Parent { get; }
		IDirectoryInfo Root { get; }

		void Create();
		void Create( DirectorySecurity directorySecurity );
		IDirectoryInfo CreateSubdirectory( string path );
		IDirectoryInfo CreateSubdirectory( string path, DirectorySecurity directorySecurity );
		void Delete( bool recursive );
		IEnumerable<IDirectoryInfo> EnumerateDirectories();
		IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern );
		IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern, SearchOption searchOption );
		IEnumerable<IFileInfo> EnumerateFiles();
		IEnumerable<IFileInfo> EnumerateFiles( string searchPattern );
		IEnumerable<IFileInfo> EnumerateFiles( string searchPattern, SearchOption searchOption );
		IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos();
		IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern );
		IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern, SearchOption searchOption );
		DirectorySecurity GetAccessControl();
		DirectorySecurity GetAccessControl( AccessControlSections includeSections );
		IDirectoryInfo[] GetDirectories();
		IDirectoryInfo[] GetDirectories( string searchPattern );
		IDirectoryInfo[] GetDirectories( string searchPattern, SearchOption searchOption );
		IFileInfo[] GetFiles();
		IFileInfo[] GetFiles( string searchPattern );
		IFileInfo[] GetFiles( string searchPattern, SearchOption searchOption );
		IFileSystemInfo[] GetFileSystemInfos();
		IFileSystemInfo[] GetFileSystemInfos( string searchPattern );
		IFileSystemInfo[] GetFileSystemInfos( string searchPattern, SearchOption searchOption );
		void MoveTo( string destDirName );
		void SetAccessControl( DirectorySecurity directorySecurity );
	}

	public class DirectoryInfo : FileSystemInfoBase<DirectoryInfoBase>, IDirectoryInfo
	{
		readonly Func<FileSystemInfoBase, IFileInfo> createFile;
		readonly Func<FileSystemInfoBase, IFileSystemInfo> createGeneral;
		readonly Func<DirectoryInfoBase, IDirectoryInfo> createDirectory;

		public DirectoryInfo( DirectoryInfoBase source ) : this( source, Defaults.Factory ) {}

		public DirectoryInfo( DirectoryInfoBase source, Func<FileSystemInfoBase, IFileSystemInfo> factory ) : base( source, factory )
		{
			createFile = Create<IFileInfo>;
			createGeneral = Create;
			createDirectory = Create<IDirectoryInfo>;
			Parent = createDirectory( Source.Parent );
			Root = createDirectory( Source.Root );
		}

		public IDirectoryInfo Parent { get; }

		public IDirectoryInfo Root { get; }

		public void Create() => Source.Create();

		public void Create( DirectorySecurity directorySecurity ) => Source.Create( directorySecurity );

		public IDirectoryInfo CreateSubdirectory( string path ) => Create<IDirectoryInfo>( Source.CreateSubdirectory( path ) );

		public IDirectoryInfo CreateSubdirectory( string path, DirectorySecurity directorySecurity ) => Create<IDirectoryInfo>( Source.CreateSubdirectory( path, directorySecurity ) );

		public void Delete( bool recursive ) => Source.Delete( recursive );

		public IEnumerable<IDirectoryInfo> EnumerateDirectories()
		{
			foreach ( var directory in Source.EnumerateDirectories() )
			{
				yield return Create<IDirectoryInfo>( directory );
			}
		}

		public IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern )
		{
			foreach ( var directory in Source.EnumerateDirectories( searchPattern ) )
			{
				yield return Create<IDirectoryInfo>( directory );
			}
		}

		public IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern, SearchOption searchOption )
		{
			foreach ( var directory in Source.EnumerateDirectories( searchPattern, searchOption ) )
			{
				yield return Create<IDirectoryInfo>( directory );
			}
		}

		public IEnumerable<IFileInfo> EnumerateFiles()
		{
			foreach ( var directory in Source.EnumerateFiles() )
			{
				yield return Create<IFileInfo>( directory );
			}
		}

		public IEnumerable<IFileInfo> EnumerateFiles( string searchPattern )
		{
			foreach ( var directory in Source.EnumerateFiles( searchPattern ) )
			{
				yield return Create<IFileInfo>( directory );
			}
		}

		public IEnumerable<IFileInfo> EnumerateFiles( string searchPattern, SearchOption searchOption )
		{
			foreach ( var directory in Source.EnumerateFiles( searchPattern, searchOption ) )
			{
				yield return Create<IFileInfo>( directory );
			}
		}

		public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
		{
			foreach ( var directory in Source.EnumerateFileSystemInfos() )
			{
				yield return Create( directory );
			}
		}

		public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern )
		{
			foreach ( var directory in Source.EnumerateFileSystemInfos( searchPattern ) )
			{
				yield return Create( directory );
			}
		}

		public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern, SearchOption searchOption )
		{
			foreach ( var directory in Source.EnumerateFileSystemInfos( searchPattern, searchOption ) )
			{
				yield return Create( directory );
			}
		}

		public DirectorySecurity GetAccessControl() => Source.GetAccessControl();

		public DirectorySecurity GetAccessControl( AccessControlSections includeSections ) => Source.GetAccessControl( includeSections );

		public IDirectoryInfo[] GetDirectories() => Source.GetDirectories().Select( createDirectory ).ToArray();
		public IDirectoryInfo[] GetDirectories( string searchPattern ) => Source.GetDirectories( searchPattern ).Select( createDirectory ).ToArray();
		public IDirectoryInfo[] GetDirectories( string searchPattern, SearchOption searchOption ) => Source.GetDirectories( searchPattern, searchOption ).Select( createDirectory ).ToArray();

		public IFileInfo[] GetFiles() => Source.GetFiles().Select( createFile ).ToArray();
		public IFileInfo[] GetFiles( string searchPattern ) => Source.GetFiles( searchPattern ).Select( createFile ).ToArray();
		public IFileInfo[] GetFiles( string searchPattern, SearchOption searchOption ) => Source.GetFiles( searchPattern, searchOption ).Select( createFile ).ToArray();

		public IFileSystemInfo[] GetFileSystemInfos() => Source.GetFileSystemInfos().Select( createGeneral ).ToArray();
		public IFileSystemInfo[] GetFileSystemInfos( string searchPattern ) => Source.GetFileSystemInfos( searchPattern ).Select( createGeneral ).ToArray();
		public IFileSystemInfo[] GetFileSystemInfos( string searchPattern, SearchOption searchOption ) => Source.GetFileSystemInfos( searchPattern, searchOption ).Select( createGeneral ).ToArray();

		public void MoveTo( string destDirName ) => Source.MoveTo( destDirName );

		public void SetAccessControl( DirectorySecurity directorySecurity ) => Source.SetAccessControl( directorySecurity );
	}
}