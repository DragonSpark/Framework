using DragonSpark.Windows.FileSystem;

namespace DragonSpark.Windows.Setup
{
	public static class Extensions
	{
		public static T Refreshed<T>( this T @this ) where T : IFileSystemInfo
		{
			@this.Refresh();
			return @this;
		}
	}
}