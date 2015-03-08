using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Configuration
{
	[ContentProperty("Items")]
	public class CollectionOfTExtension : CollectionOfTExtensionBase<IList>
	{
		protected override Type GetCollectionType(Type type)
		{
			return typeof(Collection<>).MakeGenericType(type);
		}
	}
}