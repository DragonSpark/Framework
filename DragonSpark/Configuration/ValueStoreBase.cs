using System.Collections.ObjectModel;

namespace DragonSpark.Configuration
{
	public abstract class ValueStoreBase : KeyedCollection<string, Registration>, IValueStore
	{
		protected override string GetKeyForItem( Registration item ) => item.Key;

		public object Get( string key )
		{
			foreach ( var item in this )
			{
				if ( item.Equals( key ) )
				{
					return item.Value;
				}
			}
			return null;
		}
	}
}