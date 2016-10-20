using DragonSpark.TypeSystem;
using System.Collections.Generic;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/*public sealed class FileSystemFactory : ParameterizedSourceBase<IEnumerable<string>, DirectoryInfoBase>
	{
		public static FileSystemFactory Default { get; } = new FileSystemFactory();
		FileSystemFactory() : this( FileElement.Empty() ) {}

		readonly FileElement file;

		public FileSystemFactory( FileElement file )
		{
			this.file = file;
		}

		public override DirectoryInfoBase Get( IEnumerable<string> parameter )
		{
			/*var files = new Dictionary<string, FileElement>();
			foreach ( var path in parameter )
			{
				files.Add( path, file );
			}#1#
			var fileSystem = new FileSystem();
			var result = fileSystem.FromDirectoryName( "." );
			return result;
		}
	}*/

	public struct FileSystemProfile
	{
		public FileSystemProfile( IEnumerable<FileEntry> entries ) : this( Items<string>.Default, entries ) {}

		public FileSystemProfile( IEnumerable<string> directories, IEnumerable<FileEntry> entries )
		{
			Directories = directories;
			Entries = entries;
		}

		public IEnumerable<string> Directories { get; set; }
		public IEnumerable<FileEntry> Entries { get; set; }
	}

	public struct FileEntry
	{
		public FileEntry( string id, IFileElement element )
		{
			Id = id;
			Element = element;
		}

		public string Id { get; }
		public IFileElement Element { get; }
	}
}
