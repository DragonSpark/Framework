using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Io
{
	public static class FileSystemInfoExtensions
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

		public static TFileSystemInfo Slide<TFileSystemInfo>( this TFileSystemInfo target, DirectoryInfo root, DirectoryInfo destination ) where TFileSystemInfo : FileSystemInfo
		{
			var path = target.FullName.Replace( root.FullName, destination.FullName );
			var result = Activator.CreateInstance( typeof(TFileSystemInfo), path ).To<TFileSystemInfo>();
			return result;
		}

		static readonly Dictionary<FileInfo,string> Cache = new Dictionary<FileInfo, string>( FileSystemInfoComparer.File );

		public static string ResolveChecksum( this FileInfo fileInfo )
		{
			var result = DictionaryExtensions.Ensure<FileInfo, string>( Cache, fileInfo, BuildChecksum );
			return result;
		}

		static string BuildChecksum( FileInfo fileInfo )
		{
			using ( var md5 = MD5.Create() )
			{
				var builder = new StringBuilder();
				using ( var stream = File.OpenRead( fileInfo.FullName ) )
				{
					var query = md5.ComputeHash( stream ).Select( x => x.ToString( "x2" ) );
					query.Apply( item => builder.Append( item ) );
					var result = builder.ToString();
					return result;
				}
			}
		}

		public static string MakeSafeFilePath( this string target, string replace = "" )
		{
			Path.GetInvalidFileNameChars().Apply( c => target = target.Replace( c.ToString(), replace ) );
			return target;
		}

		public static string ToUri( this string target )
		{
			var result = target.Replace( Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar );
			return result;
		}
	}
}
