using System;
using System.IO;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IFileSystemElement
	{
		FileAttributes Attributes { get; set; }
		DateTimeOffset CreationTime { get; set; }
		DateTimeOffset LastAccessTime { get; set; }
		DateTimeOffset LastWriteTime { get; set; }
	}
}