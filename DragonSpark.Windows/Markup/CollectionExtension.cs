using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	[ContentProperty( "Items" )]
	public class CollectionExtension : MonitoredMarkupExtension
	{
		public Collection Items { get; } = new Collection();

		protected virtual IList DetermineCollection( IServiceProvider serviceProvider )
		{
			var service = serviceProvider.Get<IProvideValueTarget>();
			var target = service.TargetObject.BuildUp();
			var result = service.TargetProperty.AsTo<PropertyInfo, IList>( source => (IList)source.GetValue( target ) ) ?? target as IList;
			return result;
		}

		protected override object GetValue( IServiceProvider serviceProvider )
		{
			var result = DetermineCollection( serviceProvider ).With( o =>
			{
				var type = o.GetType().Extend().GetEnumerableType();
				Items.Where( type.IsInstanceOfType ).Apply( item =>
				{
					o.Add( item );
				} );
				Items.Clear();
			} );
			return result;
		}
	}
}