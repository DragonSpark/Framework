using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ScrollIntoView : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			AssociatedObject.EnsureLoaded( x => Refresh() );
			base.OnAttached();
		}

		public bool Enabled
		{
			get { return GetValue( EnabledProperty ).To<bool>(); }
			set { SetValue( EnabledProperty, value ); }
		}	public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register( "Enabled", typeof(bool), typeof(ScrollIntoView), new PropertyMetadata( ( sender, args) => sender.To<ScrollIntoView>().Refresh() ) );


		void Refresh()
		{
			Dispatcher.BeginInvoke( () => Enabled.IsTrue( () => AssociatedObject.GetParentOfType<ScrollViewer>().NotNull( y => y.ScrollIntoView( AssociatedObject ) ) ) );
		}
	}
}