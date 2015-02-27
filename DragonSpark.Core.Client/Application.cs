using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace DragonSpark.Client
{
	public class Application : Xamarin.Forms.Application
	{
		public FileImageSource Logo
		{
			get { return (FileImageSource)GetValue( LogoProperty ); }
			set { SetValue( LogoProperty, value ); }
		}	public static readonly BindableProperty LogoProperty = BindableProperty.Create( "Logo", typeof(FileImageSource), typeof (Application), default(FileImageSource) );

		public ICollection<ToolbarItem> Items
		{
			get { return (ICollection<ToolbarItem>)GetValue( ItemsProperty ); }
			// set { SetValue( ItemsProperty, value ); }
		}	public static readonly BindableProperty ItemsProperty = BindableProperty.Create( "Items", typeof(IEnumerable<ToolbarItem>), typeof (Application), new ObservableCollection<ToolbarItem>() );
	}
}