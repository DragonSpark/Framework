using DragonSpark.Activation;
using System;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public static class Defaults
	{
		public const string 
			AssemblyExtension = ".dll",
			ValidPathTimeFormat = "yyyy-MM-dd--HH-mm-ss";

		public static Func<FileSystemInfoBase, IFileSystemInfo> General { get; } = Factory.Default.Get;
		public static Func<DirectoryInfoBase, IDirectoryInfo> Directory { get; } = ParameterConstructor<DirectoryInfoBase, DirectoryInfo>.Default;
		public static Func<FileInfoBase, IFileInfo> File { get; } = ParameterConstructor<FileInfoBase, FileInfo>.Default;
	}
}