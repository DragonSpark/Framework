using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DragonSpark.Client
{
	public class Application : Xamarin.Forms.Application
	{
		public ICollection<ToolbarItem> Items
		{
			get { return (ICollection<ToolbarItem>)GetValue( ItemsProperty ); }
			// set { SetValue( ItemsProperty, value ); }
		}	public static readonly BindableProperty ItemsProperty = BindableProperty.Create( "Items", typeof(IEnumerable<ToolbarItem>), typeof (Application), new ObservableCollection<ToolbarItem>() );
	}
	

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