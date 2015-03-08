using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ActualSizeBehavior : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			AssociatedObject.SizeChanged += AssociatedObjectSizeChanged;
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.SizeChanged -= AssociatedObjectSizeChanged;
			base.OnDetaching();
		}

		public Size ActualSize
		{
			get { return GetValue( ActualSizeProperty ).To<Size>(); }
			set { SetValue( ActualSizeProperty, value ); }
		}	public static readonly DependencyProperty ActualSizeProperty = DependencyProperty.Register( "ActualSize", typeof(Size), typeof(ActualSizeBehavior), new PropertyMetadata( new Size( 0, 0 ) ) );


		void AssociatedObjectSizeChanged(object sender, SizeChangedEventArgs e)
		{
			Dispatcher.BeginInvoke( () =>
			                        	{
			                        		ActualSize = new Size( AssociatedObject.ActualWidth, AssociatedObject.ActualHeight );
			                        	} );
			
		}
	}
}