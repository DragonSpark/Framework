using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
					OnPropertyChanging( "Item" );

					item = value;

					OnPropertyChanged( "Item" );
				}
			}
		}	T item;

		public static implicit operator T( Container<T> container )
		{
			return container.Item;
		}
	}

	public class HelloWorldExtension : IMarkupExtension
	{
		/*readonly string message;

		public HelloWorldExtension( string message )
		{
			this.message = message;
		}*/

		public string Message { get; set; }

		public object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = string.Format( "Hello World! Message: {0}", Message );
			return result;
		}
	}
}