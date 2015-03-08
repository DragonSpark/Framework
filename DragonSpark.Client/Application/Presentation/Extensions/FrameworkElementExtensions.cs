using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using ChildWindow = System.Windows.Controls.ChildWindow;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static partial class FrameworkElementExtensions
	{
		public static bool IsAttachedToScene( this FrameworkElement target )
		{
			var root = target.GetVisualAncestorsAndSelf().LastOrDefault();
			var result = root is ChildWindow || root == System.Windows.Application.Current.GetShell();
			return result;
		}
	}
}