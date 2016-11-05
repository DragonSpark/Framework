using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Windows.FileSystem;
using System;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public static class Extensions
	{
		public static IFileElement GetFile( this IFileSystemRepository @this, string path ) => @this.Get( path )?.AsValid<IFileElement>();
		public static IDirectoryElement GetDirectory( this IFileSystemRepository @this, string path ) => @this.Get( path )?.AsValid<IDirectoryElement>();

		public static string AsText( this IFileElement @this ) => MockFile.ReadAllBytes( @this.Unwrap(), Defaults.DefaultEncoding );

		public static void IsLegalAbsoluteOrRelative(this IPath @this, string pathToValidate, string paramName)
		{
			if (pathToValidate.Trim() == string.Empty)
			{
				throw new ArgumentException(Properties.Resources.THE_PATH_IS_NOT_OF_A_LEGAL_FORM, paramName);
			}


			if ( !@this.IsValidFileName( @this.GetFileName( pathToValidate ) ) )
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
			}

			
			if ( !@this.IsValidPath( @this.GetDirectoryName( pathToValidate ) ) )
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
			}
		}

		public static string EnsureTrailingSlash( this IPath @this, string path, string reference = null )
		{
			var separator = @this.DirectorySeparatorChar;
			var separatorText = separator.ToString();
			var result = reference?.EndsWith( separatorText, StringComparison.OrdinalIgnoreCase ) ?? true ? string.Concat( path.TrimEnd( separator ), separatorText ) : path;
			return result;
		}

		public static string Normalize( this IPath @this, string parameter )
		{
			var path = parameter.Replace( @this.AltDirectorySeparatorChar, @this.DirectorySeparatorChar );
			var fullPath = @this.GetFullPath( path );
			var result = @this.HasExtension( parameter ) ? fullPath : NormalizeDirectory( @this, fullPath );
			return result;
		}

		static string NormalizeDirectory( IPath path, string fullPath ) => 
			string.Equals( fullPath, path.GetPathRoot( fullPath ), StringComparison.OrdinalIgnoreCase ) 
			? 
			fullPath 
			:
			fullPath.TrimEnd( path.DirectorySeparatorChar, path.AltDirectorySeparatorChar );
	}
}