using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Items" )]
	public class GenericEnumerableFactory : GenericEnumerableFactoryBase
	{
		public Collection<object> Items
		{
			get { return items; }
		}	readonly Collection<object> items = new Collection<object>();

		protected override System.Collections.Generic.IEnumerable<TItem> Create<TItem>( IUnityContainer container, string buildName )
		{
			var result = Items.Select( x => x.ResolvedAs<TItem>( () => container ) ).NotNull().ToArray();
			return result;
		}
	}
}