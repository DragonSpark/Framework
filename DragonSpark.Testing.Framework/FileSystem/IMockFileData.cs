using DragonSpark.Sources;
using System;
using System.Collections.Immutable;
using System.IO;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IFileSystemElement
	{
		// string Path { get; }

		FileAttributes Attributes { get; set; }
		DateTimeOffset CreationTime { get; set; }
		DateTimeOffset LastAccessTime { get; set; }
		DateTimeOffset LastWriteTime { get; set; }
	}

	public interface IFileElement : IFileSystemElement, IAssignableSource<ImmutableArray<byte>> {}

	public interface IDirectoryElement : IFileSystemElement {}
}