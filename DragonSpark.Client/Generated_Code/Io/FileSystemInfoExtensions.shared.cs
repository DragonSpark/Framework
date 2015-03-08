using System;
using System.IO;
using DragonSpark.Extensions;

namespace DragonSpark.Io
{
	public static partial class FileSystemInfoExtensions
	{
		public static TFileSystemInfo Move<TFileSystemInfo>( this TFileSystemInfo target, string locationRoot, string name = null ) where TFileSystemInfo : FileSystemInfo
		{
			target.As<DirectoryInfo>( x =>
			{
				var destination = Path.Combine( locationRoot, name ?? target.Name );
				Directory.CreateDirectory( locationRoot );
				x.MoveTo( destination );
			} );
			target.As<FileInfo>( x =>
			{
				var destination = Path.Combine( locationRoot, name.Transform( y => string.Concat( y, Path.GetExtension( x.Name ) ), () => x.Name ) );
				var directoryName = Path.GetDirectoryName( destination );
				Directory.CreateDirectory( directoryName );
				x.MoveTo( destination );
			} );
			return target;
		}

		public static TFileSystemInfo Slide<TFileSystemInfo>( this TFileSystemInfo target, DirectoryInfo destination ) where TFileSystemInfo : FileSystemInfo
		{
			return Slide( target, target.As<DirectoryInfo>(), destination );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Want to ensure a directory is passed in to resolve the full path from.  FileInfo will cause an error." )]
		public static TFileSystemInfo Slide<TFileSystemInfo>( this TFileSystemInfo target, DirectoryInfo root, DirectoryInfo destination ) where TFileSystemInfo : FileSystemInfo
		{
			var path = target.FullName.Replace( root.FullName, destination.FullName );
			var result = Activator.CreateInstance( typeof(TFileSystemInfo), path ).To<TFileSystemInfo>();
			return result;
		}
	}
}