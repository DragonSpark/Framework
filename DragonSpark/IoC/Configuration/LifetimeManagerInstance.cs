using System.Windows.Markup;
using DragonSpark.Configuration;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Item" )]
	public class LifetimeManagerInstance : Singleton<Microsoft.Practices.Unity.LifetimeManager>
	{
		public Microsoft.Practices.Unity.LifetimeManager Item { get; set; }

		protected override Microsoft.Practices.Unity.LifetimeManager Create()
		{
			return Item;
		}
	}
}