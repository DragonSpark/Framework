using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace DragonSpark.Application.Presentation.Behaviors
{
	public class ScrollToLastLineBehavior : Behavior<TextBoxBase>
	{
		protected override void OnAttached()
		{
			AssociatedObject.TextChanged += AssociatedObjectTextChanged;
			base.OnAttached();
		}

		void AssociatedObjectTextChanged( object sender, TextChangedEventArgs e )
		{
			AssociatedObject.ScrollToEnd();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.TextChanged -= AssociatedObjectTextChanged;
			base.OnDetaching();
		}
	}
}
