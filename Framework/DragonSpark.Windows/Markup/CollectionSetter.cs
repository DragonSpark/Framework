using System.Collections;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	public class CollectionSetter : MarkupTargetValueSetterBase
	{
		readonly IList collection;
		
		public CollectionSetter( IList collection )
		{
			this.collection = collection;
		}

		protected override void Apply( object value )
		{
			var item = collection.Cast<object>().FirstOrDefault( o => o == null );
			var index = collection.IndexOf( item );
			collection.RemoveAt( index );

			var itemType = collection.GetType().Extend().GetInnerType();
			var items = itemType.IsInstanceOfType( value ) ? value.AsItem() : value is IEnumerable && itemType.IsAssignableFrom( value.GetType().Extend().GetInnerType() ) ? value.To<IEnumerable>().Cast<object>().Reverse() : Enumerable.Empty<object>();
			items.Each( o => collection.Insert( index, o ) );
		}
	}
}