using System;
using System.Collections.Generic;
using System.IO;
using DragonSpark.Extensions;

namespace DragonSpark.Io
{
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