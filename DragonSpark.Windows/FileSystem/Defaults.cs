using System;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public static class Defaults
	{
		public const string 
			AssemblyExtension = ".dll",
			ValidPathTimeFormat = "yyyy-MM-dd--HH-mm-ss";

		public static Func<FileSystemInfoBase, IFileSystemInfo> Factory { get; } = FileSystem.Factory.Default.Get;
	}
}