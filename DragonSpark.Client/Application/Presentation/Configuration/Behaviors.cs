using System.Windows;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Configuration
{
	public static class Behaviors
	{
		public static readonly DependencyProperty ItemsProperty =
			DependencyProperty.RegisterAttached( "Items", typeof(BehaviorCollection), typeof(Behaviors),
			                                     new PropertyMetadata( OnItemsPropertyChanged ) );

		static void OnItemsPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			var items = System.Windows.Interactivity.Interaction.GetBehaviors( o );
			e.NewValue.As<BehaviorCollection>( x => x.Apply( y => items.Add( y.Clone() ) ) );
		}

		public static BehaviorCollection GetItems( FrameworkElement element )
		{
			return (BehaviorCollection)element.GetValue( ItemsProperty );
		}

		public static void SetItems( FrameworkElement element, BehaviorCollection value )
		{
			element.SetValue( ItemsProperty, value );
		}
	}
}