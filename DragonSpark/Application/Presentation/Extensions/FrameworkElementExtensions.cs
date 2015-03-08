using System.Windows;
using System.Windows.Media;
using DragonSpark.Application.Presentation.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
    public static partial class FrameworkElementExtensions
    {
        public static bool IsAttachedToScene( this DependencyObject target )
        {
            var result = target.As<DialogChrome>().Transform( x => x.Visibility != Visibility.Collapsed ) || target == System.Windows.Application.Current.GetShell() || VisualTreeHelper.GetParent( target ) != null;
            return result;
        }
    }
}
