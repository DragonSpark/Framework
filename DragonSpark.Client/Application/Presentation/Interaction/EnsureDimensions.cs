using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DragonSpark.Application.Presentation.Interaction
{
	/*public class EventTrigger : System.Windows.Interactivity.EventTrigger
	{
		protected override void OnAttached()
		{
			AssociatedObject.As<FrameworkElement>( x => x.EnsureLoaded( y => base.OnAttached() ) );
		}
	}*/

	public class EnsureDimensions : Behavior<ComboBox>
	{
		protected override void OnAttached()
		{
			AssociatedObject.LayoutUpdated += AssociatedObjectLayoutUpdated;
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.LayoutUpdated -= AssociatedObjectLayoutUpdated;
			base.OnDetaching();
		}

		public Size MaxItemSize
		{
			get { return GetValue( MaxItemSizeProperty ).To<Size>(); }
			set { SetValue( MaxItemSizeProperty, value ); }
		}	public static readonly DependencyProperty MaxItemSizeProperty = DependencyProperty.Register( "MaxItemSize", typeof(Size), typeof(EnsureDimensions), null );

		void AssociatedObjectLayoutUpdated( object sender, EventArgs e )
		{
			var items = AssociatedObject.ToEnumerable( AssociatedObject.Items.OfType<FrameworkElement>() );
			MaxItemSize = new Size( items.Max( x => x.ActualWidth ), items.Max( x => x.ActualHeight ) );
		}
	}
}