using DragonSpark.Extensions;
using DragonSpark.Sources;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public static class Extensions
	{
		public static IFileElement GetFile( this IFileSystemRepository @this, string path ) => @this.GetElement( path ).AsValid<IFileElement>();
		public static IDirectoryElement GetDirectory( this IFileSystemRepository @this, string path ) => @this.GetElement( path ).AsValid<IDirectoryElement>();

		public static string AsText( this IFileElement @this ) => MockFile.ReadAllBytes( @this.ToArray(), Defaults.DefaultEncoding );
	}
}