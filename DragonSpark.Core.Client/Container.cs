using Xamarin.Forms;

namespace DragonSpark.Client
{
	[ContentProperty( "Item" )]
	public abstract class Container<T> : BindableObject
	{
		public T Item
		{
			get { return item; }
			set
			{
				if ( !Equals( item, value ) )
				{
					OnPropertyChanging();

					item = value;

					OnPropertyChanged();
				}
			}
		}	T item;

		public static implicit operator T( Container<T> container )
		{
			return container.Item;
		}
	}
}