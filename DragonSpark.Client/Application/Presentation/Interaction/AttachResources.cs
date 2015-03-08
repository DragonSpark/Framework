using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
    public class AttachResources : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Resources.Values.OfType<IAttachedObject>().Apply( x => x.Attach( AssociatedObject ) );
        }
    }
}