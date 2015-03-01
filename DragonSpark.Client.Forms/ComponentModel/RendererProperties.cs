using System.Windows;

namespace DragonSpark.Application.Client.Forms.ComponentModel
{
	public static class RendererProperties
	{
		public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached( "Title", typeof(string), typeof(RendererProperties), new PropertyMetadata( OnTitlePropertyChanged ) );

		static void OnTitlePropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		public static string GetTitle( FrameworkElement element )
		{
			return (string)element.GetValue( TitleProperty );
		}

		public static void SetTitle( FrameworkElement element, string value )
		{
			element.SetValue( TitleProperty, value );
		}

		/*public static readonly DependencyProperty DialogProperty = DependencyProperty.RegisterAttached( "Dialog", typeof(UIElement), typeof(ShellProperties), new PropertyMetadata( OnDialogPropertyChanged ) );

		static void OnDialogPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		public static Window GetDialog( UIElement element )
		{
			return (Window)element.GetValue( DialogProperty );
		}

		public static void SetDialog( UIElement element, Window value )
		{
			element.SetValue( DialogProperty, value );
		}*/
	}
}