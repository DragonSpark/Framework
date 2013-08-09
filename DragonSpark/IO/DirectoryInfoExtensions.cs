using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Linq;
using SearchOption = System.IO.SearchOption;

namespace DragonSpark.Io
{
	public static class DirectoryInfoExtensions
	{
		public static string DetermineRelative( this DirectoryInfo target, FileSystemInfo @from, bool adjustSlashes = true )
		{
			var result = Uri.UnescapeDataString( new Uri(target.FullName).MakeRelativeUri( new Uri( @from.FullName ) ).ToString().Transform( x => adjustSlashes ? x.Replace( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar) : x ) );
			return result;
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
				files.Apply( x => Logging.Log.Try( x.Delete ) );
			}
		}

		public static FileInfo[] GetAllFiles( this DirectoryInfo target )
		{
			var result = target.GetFiles( "*.*", SearchOption.AllDirectories );
			return result;
		}

		public static long GetTotalSize( this DirectoryInfo target )
		{
			var result = Enumerable.Sum<FileInfo>( target.GetAllFiles(), x => x.Length );
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This method throws many types of exceptions." ), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Any other FileSystemInfo object will cause an error.")]
		public static DirectoryInfo CopyTo( this DirectoryInfo target, string destinationPath )
		{
			try
			{
				FileSystem.CopyDirectory( target.FullName, destinationPath );
			}
			catch ( Exception e )
			{
				var items = e.Data.Values.Cast<object>().ToArray();
				var message = string.Join( System.Environment.NewLine, items.Select( x => x.ToString() ) );
				Logging.Log.Warning( string.Format( Resources.Message_Logging_DirectoryInfo, target.Name, System.Environment.NewLine, message ) );
			}
			var result = new DirectoryInfo( destinationPath );
			return result;
		}
	}
}
