using System.Windows;

namespace Microsoft.Expression.Interactions.Extensions {

	/// <summary>
	/// Collection of helper methods.
	/// </summary>
	public static class Helpers {

		/// <summary>
		/// Helper to attempt to detect if an element is loaded or not.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool IsLoaded(this FrameworkElement element) {
#if SILVERLIGHT
			UIElement rootVisual = Application.Current.RootVisual;
			DependencyObject parent = element.Parent;
			if (parent == null) {
				parent = VisualTreeHelper.GetParent(element);
			}
			return ((parent != null) || ((rootVisual != null) && (element == rootVisual)));
#else
			return element.IsLoaded;
#endif
		}
	}
}
