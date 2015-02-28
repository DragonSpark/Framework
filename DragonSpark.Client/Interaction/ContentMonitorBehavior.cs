using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Client.Interaction
{
	public class ContentMonitorBehavior : Behavior<ContentControl>
	{
		public object Content
		{
			get { return GetValue( ContentProperty ).To<object>(); }
			set { SetValue( ContentProperty, value ); }
		}	public static readonly DependencyProperty ContentProperty = DependencyProperty.Register( "Content", typeof(object), typeof(ContentMonitorBehavior), new PropertyMetadata( ( o, args ) => o.As<ContentMonitorBehavior>( x => OnContentChanged( args.NewValue ) ) ) );

		static void OnContentChanged( object newValue )
		{
			newValue.As<FrameworkElement>( element =>
			{
				if ( element.ReadLocalValue( FrameworkElement.DataContextProperty ) == DependencyProperty.UnsetValue )
				{
					element.DataContext = element;
				}
			} );			
		}
	}
}