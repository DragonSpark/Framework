using DragonSpark.TypeSystem;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public struct FileSystemRegistration
	{
		public static FileSystemRegistration File( string path ) => new FileSystemRegistration( path, new FileElement( Items<byte>.Default ) );
		public static FileSystemRegistration Directory( string path ) => new FileSystemRegistration( path, new DirectoryElement() );

		public FileSystemRegistration( string path, ImmutableArray<byte> bytes ) : this( path, new FileElement( bytes ) ) {}
		public FileSystemRegistration( string path, IEnumerable<byte> bytes ) : this( path, new FileElement( bytes ) ) {}

		public FileSystemRegistration( string path, IFileSystemElement element )
		{
			Path = path;
			Element = element;
		}

		public string Path { get; }
		public IFileSystemElement Element { get; }
	}
}