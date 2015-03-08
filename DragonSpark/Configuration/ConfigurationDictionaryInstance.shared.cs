using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using Caliburn.Micro;

namespace Common.Configuration
{
	[ContentProperty( "Items" )]
	public class ConfigurationDictionaryInstance<TKey, TValue> : InstanceProviderBase<Dictionary<TKey,TValue>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used for generic configuration." )]
		public IObservableCollection<ConfigurationDictionaryItem<TKey,TValue>> Items
		{
			get { return items ?? ( items ); }
		}	readonly IObservableCollection<ConfigurationDictionaryItem<TKey,TValue>> items  = new BindableCollection<ConfigurationDictionaryItem<TKey,TValue>>();

		protected override Dictionary<TKey, TValue> Create()
		{
			var result = Items.ToDictionary( x => x.ItemKey, x => x.Value );
			return result;
		}
	}
}