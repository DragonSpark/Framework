using Xamarin.Forms;

namespace DragonSpark.Application.Forms
{
	public static class Compensations
	{
		public static void Assign( this Page @this, IPlatform item )
		{
			@this.Platform = item;
		}

		public static void AssignNavigation( this Page @this, INavigation navigation )
		{
			@this.NavigationProxy.Inner = navigation;
		}

		public static INavigation GetNavigation( this Page @this )
		{
			return @this.NavigationProxy.Inner;
		}
	}
}