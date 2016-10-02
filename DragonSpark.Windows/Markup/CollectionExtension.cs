using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Collections;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	[ContentProperty( nameof(Items) )]
	public class CollectionExtension : MarkupExtensionBase
	{
		public DeclarativeCollection Items { get; } = new DeclarativeCollection();

		protected virtual IList DetermineList( MarkupServiceProvider serviceProvider )
		{
			var target = serviceProvider.TargetObject;
			var result = serviceProvider.Property.GetValue() as IList ?? target as IList;
			return result;
		}

		protected override object GetValue( MarkupServiceProvider serviceProvider )
		{
			var result = DetermineList( serviceProvider );
			if ( result != null )
			{
				var type = result.GetType().Adapt().GetEnumerableType();
				foreach ( var source in Items.Purge().Where( type.IsInstanceOfType ) )
				{
					result.Add( source );
				}
			}
			return result;
		}
	}
}