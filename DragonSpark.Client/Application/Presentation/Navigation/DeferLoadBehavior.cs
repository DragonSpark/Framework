using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Navigation;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Navigation
{
    public class DeferLoadBehavior : Behavior<Frame>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.NavigationFailed += AssociatedObjectOnNavigationFailed;
        }

        void AssociatedObjectOnNavigationFailed( object sender, NavigationFailedEventArgs navigationFailedEventArgs )
        {
            var handled = AssociatedObject.ContentLoader.AsTo<FrameContentLoader, bool?>( x => !x.IsReady() ) ?? true;
            navigationFailedEventArgs.Handled |= handled;
            // ready.IsFalse( () => Threading.Application.Start( AssociatedObject.RefreshFromState ) );
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.NavigationFailed -= AssociatedObjectOnNavigationFailed;
        }
    }
}