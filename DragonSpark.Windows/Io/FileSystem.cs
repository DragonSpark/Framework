using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Globalization;
using System.IO;

namespace DragonSpark.Windows.Io
{
	public static class FileSystem
	{
		public const string ValidPathTimeFormat = "yyyy-M-dd--HH-mm-ss";

		public static string GetValidPath()
		{
			return GetValidPath( CurrentTime.Instance );
		}

		public static string GetValidPath( ICurrentTime @this )
		{
			var result = @this.Now.ToString( ValidPathTimeFormat );
			return result;
		}

		public static bool IsValidPath( string input )
		{
			DateTimeOffset item;
			var result = DateTimeOffset.TryParseExact( input, ValidPathTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out item );
			return result;
		}

		public static bool IsLocked( this FileInfo @this )
		{
			FileStream stream = null;

			try
			{
				stream = @this.Open( FileMode.Open, FileAccess.Read, FileShare.None );
			}
			catch ( IOException )
			{
				return true;
			}
			finally
			{
				stream?.Close();
			}

			return false;
		}

		public static DirectoryInfo Purge( this DirectoryInfo target )
		{
			target.Exists.IsTrue( () =>
			{
				target.GetDirectories().Each( TryDeleteDirectory );
				target.GetFiles().Each( x => x.Delete() );
			} );
			return target;
		}

		static void TryDeleteDirectory( DirectoryInfo target )
		{
			try
			{
				target.Delete( true );
			}
			catch ( IOException )
			{
				var files = target.GetAllFiles();
				files.Each( x => ExceptionSupport.Try( x.Delete ) );
			}
		}

		public static FileInfo[] GetAllFiles( this DirectoryInfo target )
		{
			var result = target.GetFiles( "*.*", SearchOption.AllDirectories );
			return result;
		}
	}
}
