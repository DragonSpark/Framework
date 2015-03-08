using System.IO;

namespace DragonSpark.Io
{
	public static class FileSystemInfoComparer
	{
		public static FileSystemInfoComparer<DirectoryInfo> Directory
		{
			get { return DirectoryField; }
		}	static readonly FileSystemInfoComparer<DirectoryInfo> DirectoryField = new FileSystemInfoComparer<DirectoryInfo>();	

		public static FileSystemInfoComparer<FileInfo> File
		{
			get { return FileField; }
		}	static readonly FileSystemInfoComparer<FileInfo> FileField = new FileSystemInfoComparer<FileInfo>();	
	}
}