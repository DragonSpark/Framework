using System;
using System.Globalization;
using System.IO;
using DragonSpark.Application;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;

namespace DragonSpark.Windows
{
	public static class FileSystem
	{
		public const string AssemblyExtension = ".dll", ValidPathTimeFormat = "yyyy-MM-dd--HH-mm-ss";

		public static string GetValidPath() => GetValidPath( CurrentTimeConfiguration.Default.Get() );

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
			if ( target.Exists )
			{
				target.GetDirectories().Each( TryDeleteDirectory );
				target.GetFiles().Each( x => x.Delete() );
			}

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
				foreach ( var file in target.GetAllFiles() )
				{
					try
					{
						file.Delete();
					}
					catch ( Exception exception )
					{
						Logger.Default.Get( file ).Error( exception, "Could not delete {File}.", file.FullName );
					}
				}
			}
		}

		public static FileInfo[] GetAllFiles( this DirectoryInfo target ) => target.GetFiles( "*.*", SearchOption.AllDirectories );
	}
}
