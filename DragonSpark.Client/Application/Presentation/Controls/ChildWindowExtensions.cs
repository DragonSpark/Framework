using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace DragonSpark.Application.Presentation.Controls
{
	public static class ChildWindowExtensions
	{
		public static void CenterInScreen(this ChildWindow target)
		{
			var count = VisualTreeHelper.GetChildrenCount( target );
			if ( count > 0 )
			{
				var root = VisualTreeHelper.GetChild( target, 0 ) as FrameworkElement;
				if ( root != null )
				{
					var contentRoot = root.FindName( "ContentRoot" ) as FrameworkElement;
					if ( contentRoot != null )
					{
						var group = contentRoot.RenderTransform as TransformGroup;
						if ( group != null )
						{
							TranslateTransform translateTransform = null;
							foreach ( var transform in group.Children.OfType<TranslateTransform>() )
							{
								translateTransform = transform;
							}

							if ( translateTransform != null )
							{
								translateTransform.X = 0.0;
								translateTransform.Y = 0.0;
							}
							// reset transform
						}
					}
				}
			}
		}
	}
}