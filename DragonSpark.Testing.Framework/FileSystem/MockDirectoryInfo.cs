using DragonSpark.Extensions;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using Directory = DragonSpark.Windows.FileSystem.Directory;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockDirectoryInfo : DirectoryInfoBase
	{
		readonly IFileSystemRepository repository;
		readonly IPath path;
		readonly IDirectory directory;
		readonly string root, parent;
		readonly IElementSource<IDirectoryElement> element;

		[UsedImplicitly]
		public MockDirectoryInfo( string directoryPath ) : this( FileSystemRepository.Current.Get(), directoryPath ) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="MockDirectoryInfo"/> class.
		/// </summary>
		/// <param name="repository">The mock file data accessor.</param>
		/// <param name="directoryPath">The directory path.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="repository"/> or <paramref name="directoryPath"/> is <see langref="null"/>.</exception>
		public MockDirectoryInfo( IFileSystemRepository repository, string directoryPath )
			: this( repository, Windows.FileSystem.Path.Current.Get(), Directory.Current.Get(), directoryPath ) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="MockDirectoryInfo"/> class.
		/// </summary>
		/// <param name="repository">The mock file data accessor.</param>
		/// <param name="path"></param>
		/// <param name="directory"></param>
		/// <param name="directoryPath">The directory path.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="repository"/> or <paramref name="directoryPath"/> is <see langref="null"/>.</exception>
		public MockDirectoryInfo( IFileSystemRepository repository, IPath path, IDirectory directory, string directoryPath ) : this( repository, path, directory, new ElementSource<IDirectoryElement>( repository, path.Normalize( directoryPath ) ) )  {}

		MockDirectoryInfo( IFileSystemRepository repository, IPath path, IDirectory directory, IElementSource<IDirectoryElement> element )
		{
			this.repository = repository;
			this.path = path;
			this.directory = directory;
			this.element = element;

			root = directory.GetDirectoryRoot( FullName );
			parent = directory.GetParent( FullName )?.FullName;
		}

		
		IDirectoryElement Element => element.Get();

		public override DirectoryInfoBase Root => Get( root );
		public override DirectoryInfoBase Parent => parent != null ? Get( parent ) : null;

		public override void Refresh() => repository.Get( FullName ).As<IDirectoryElement>( element.Assign );

		public override bool Exists => directory.Exists( FullName );

		public override string Extension => path.GetExtension( FullName );

		public sealed override string FullName => element.Path;

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

		public override DateTime LastAccessTime
		{
			get { return Element.LastAccessTime.DateTime; }
			set { Element.LastAccessTime = value; }
		}

		public override DateTime LastAccessTimeUtc
		{
			get { return Element.LastAccessTime.UtcDateTime; }
			set { Element.LastAccessTime = value.ToLocalTime(); }
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

		public override string Name => path.GetFileName( FullName.TrimEnd( path.DirectorySeparatorChar ) );

		public override void Create()
		{
			directory.CreateDirectory( FullName );
			Refresh();
		}

		public override void Create( DirectorySecurity directorySecurity )
		{
			directory.CreateDirectory( FullName, directorySecurity );
			Refresh();
		}

		public override DirectoryInfoBase CreateSubdirectory( string pathName ) => Get( directory.CreateDirectory( path.Combine( FullName, pathName ) ) );

		public override DirectoryInfoBase CreateSubdirectory( string pathName, DirectorySecurity directorySecurity ) => Get( directory.CreateDirectory( path.Combine( FullName, pathName ), directorySecurity ) );

		DirectoryInfoBase Get( IFileSystemInfo source ) => Get( source.FullName );
		DirectoryInfoBase Get( string source ) => repository.FromDirectoryName( source );

		public override void Delete()
		{
			directory.Delete( FullName );
			Refresh();
		}

		public override void Delete( bool recursive )
		{
			directory.Delete( FullName, recursive );
			Refresh();
		}

		public override IEnumerable<DirectoryInfoBase> EnumerateDirectories() => GetDirectories();

		public override IEnumerable<DirectoryInfoBase> EnumerateDirectories( string searchPattern ) => GetDirectories( searchPattern );

		public override IEnumerable<DirectoryInfoBase> EnumerateDirectories( string searchPattern, SearchOption searchOption ) => GetDirectories( searchPattern, searchOption );

		public override IEnumerable<FileInfoBase> EnumerateFiles() => GetFiles();

		public override IEnumerable<FileInfoBase> EnumerateFiles( string searchPattern ) => GetFiles( searchPattern );

		public override IEnumerable<FileInfoBase> EnumerateFiles( string searchPattern, SearchOption searchOption ) => GetFiles( searchPattern, searchOption );

		public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos() => GetFileSystemInfos();

		public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos( string searchPattern ) => GetFileSystemInfos( searchPattern );

		public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos( string searchPattern, SearchOption searchOption ) => GetFileSystemInfos( searchPattern, searchOption );

		public override DirectorySecurity GetAccessControl() => directory.GetAccessControl( FullName );

		public override DirectorySecurity GetAccessControl( AccessControlSections includeSections ) => directory.GetAccessControl( FullName, includeSections );

		public override DirectoryInfoBase[] GetDirectories() => ConvertStringsToDirectories( directory.GetDirectories( FullName ) );

		public override DirectoryInfoBase[] GetDirectories( string searchPattern ) => ConvertStringsToDirectories( directory.GetDirectories( FullName, searchPattern ) );

		public override DirectoryInfoBase[] GetDirectories( string searchPattern, SearchOption searchOption ) => ConvertStringsToDirectories( directory.GetDirectories( FullName, searchPattern, searchOption ) );

		DirectoryInfoBase[] ConvertStringsToDirectories( IEnumerable<string> paths ) => paths.Select( s => repository.FromDirectoryName( s ) ).ToArray();

		public override FileInfoBase[] GetFiles() => ConvertStringsToFiles( directory.GetFiles( FullName ) );

		public override FileInfoBase[] GetFiles( string searchPattern ) => ConvertStringsToFiles( directory.GetFiles( FullName, searchPattern ) );

		public override FileInfoBase[] GetFiles( string searchPattern, SearchOption searchOption ) => ConvertStringsToFiles( directory.GetFiles( FullName, searchPattern, searchOption ) );

		FileInfoBase[] ConvertStringsToFiles( IEnumerable<string> paths ) => paths.Select( repository.FromFileName ).ToArray();

		public override FileSystemInfoBase[] GetFileSystemInfos() => GetFileSystemInfos( Defaults.AllPattern );

		public override FileSystemInfoBase[] GetFileSystemInfos( string searchPattern ) => GetFileSystemInfos( searchPattern, SearchOption.TopDirectoryOnly );

		public override FileSystemInfoBase[] GetFileSystemInfos( string searchPattern, SearchOption searchOption ) => GetDirectories( searchPattern, searchOption ).OfType<FileSystemInfoBase>().Concat( GetFiles( searchPattern, searchOption ) ).ToArray();

		public override void MoveTo( string destDirName )
		{
			directory.Move( FullName, destDirName );
			Refresh();
		}

		public override void SetAccessControl( DirectorySecurity directorySecurity ) => directory.SetAccessControl( FullName, directorySecurity );
	}
}
