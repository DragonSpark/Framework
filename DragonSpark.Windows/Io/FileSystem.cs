using System;
using System.Globalization;
using System.IO;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Io
{
	public static class FileSystem
	{
		public const string ValidPathTimeFormat = "yyyy-M-dd--HH-mm-ss";

		public static string GetValidPath()
		{
			return GetValidPath( DateTimeOffset.Now );
		}

		public static string GetValidPath( DateTimeOffset @this )
		{
			var result = @this.ToString( FileSystem.ValidPathTimeFormat );
			return result;
		}

		public static bool IsValidPath( string input )
		{
			DateTimeOffset item;
			var result = DateTimeOffset.TryParseExact( input, FileSystem.ValidPathTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out item );
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
				target.GetDirectories().Apply( TryDeleteDirectory );
				target.GetFiles().Apply( x => x.Delete() );
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
				files.Apply( x => DiagnosticExtensions.Try( x.Delete ) );
			}
		}

		public static FileInfo[] GetAllFiles( this DirectoryInfo target )
		{
			var result = target.GetFiles( "*.*", SearchOption.AllDirectories );
			return result;
		}
	}
}
