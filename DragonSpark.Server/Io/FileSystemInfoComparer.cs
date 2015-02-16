using System;
using System.Collections.Generic;
using System.IO;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Io
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

	public class FileSystemInfoComparer<TFileSystemInfo> : IEqualityComparer<TFileSystemInfo> where TFileSystemInfo : FileSystemInfo
	{
		public bool Equals( TFileSystemInfo x, TFileSystemInfo y )
		{
			var result = string.Compare( x.Transform( i => i.FullName ), y.Transform( i => i.FullName ), StringComparison.InvariantCultureIgnoreCase ) == 0;
			return result;
		}

		public int GetHashCode( TFileSystemInfo obj )
		{
			var result = obj.Transform( item => item.FullName.GetHashCode() );
			return result;
		}
	}
}