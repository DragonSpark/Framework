using System.Windows;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class DataStateBehavior : Expression.Samples.Interactivity.DataStateBehavior
	{
		bool IsActive { get; set; }

		FrameworkElement Element { get; set; }

		// bool RefreshRequired { get; set; }

		protected override void OnAttached()
		{
			AssociatedObject.Loaded += AssociatedObjectLoaded;
			AssociatedObject.Unloaded += AssociatedObjectUnloaded;
			AssociatedObject.EnsureLoaded( Attach, false );
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Loaded -= AssociatedObjectLoaded;
			AssociatedObject.Unloaded -= AssociatedObjectUnloaded;
			base.OnDetaching();
		}

		void Attach( FrameworkElement element )
		{
			base.OnAttached();

			this.RefreshValue( ValueProperty );
		}

		void AssociatedObjectLoaded( object sender, RoutedEventArgs e )
		{
			IsActive = true;
			UpdateState( Element );
		}

		void AssociatedObjectUnloaded( object sender, RoutedEventArgs e )
		{
			IsActive = false;
		}

		protected override void UpdateState( FrameworkElement targetElement )
		{
			Element = targetElement;
			if ( IsActive && targetElement.Transform( x => x.IsAttachedToScene(), () => true ) )
			{
				base.UpdateState( targetElement );
			}
		}

		/*public object Tag
		{
			get { return GetValue( TagProperty ).To<object>(); }
			set { SetValue( TagProperty, value ); }
		}	public static readonly DependencyProperty TagProperty = DependencyProperty.Register( "Tag", typeof(object), typeof(DataStateBehavior), null );*/
	}
}