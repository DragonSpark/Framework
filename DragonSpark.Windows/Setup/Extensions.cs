using System.IO.Abstractions;

namespace DragonSpark.Windows.Setup
{
	public static class Extensions
	{
		public static T Refreshed<T>( this T @this ) where T : FileSystemInfoBase
		{
			@this.Refresh();
			return @this;
		}
	}
}