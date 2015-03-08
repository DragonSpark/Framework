using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DragonSpark.Extensions;

namespace DragonSpark.Io
{
	partial class FileSystemInfoExtensions
	{
		static readonly Dictionary<FileInfo,string> Cache = new Dictionary<FileInfo, string>( FileSystemInfoComparer.File );

		public static string ResolveChecksum( this FileInfo fileInfo )
		{
			var result = Cache.Ensure( fileInfo, BuildChecksum );
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
	}
}
