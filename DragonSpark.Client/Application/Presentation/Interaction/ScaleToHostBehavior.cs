using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using DragonSpark.Application.Presentation.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ScaleToHostBehavior : Behavior<FrameworkElement>
	{
		Size? TargetSize { get; set; }

		protected override void OnAttached()
		{
			AssociatedObject.EnsureLoaded( item =>
			{
				AssociatedObject.SizeChanged += AssociatedObjectLayoutUpdated;
				TargetSize = TargetSize ?? new Size( AssociatedObject.Width, AssociatedObject.Height );

				System.Windows.Application.Current.Host.Content.Resized += ContentResized;
			} );
			base.OnAttached();
		}

		void ContentResized( object sender, EventArgs e )
		{
			Update();
		}

		void AssociatedObjectLayoutUpdated( object sender, EventArgs e )
		{
			TargetSize = TargetSize ?? new Size( AssociatedObject.Width, AssociatedObject.Height );
			// Root.RenderTransformOrigin = new Point( .5, .5 );
			Update();
		}

		void Update()
		{

			if ( TargetSize != null )
			{
				var hostContentActualHeight = System.Windows.Application.Current.Host.Content.ActualHeight;
				var hostContentActualWidth = System.Windows.Application.Current.Host.Content.ActualWidth;

				var heightRatio = hostContentActualHeight / TargetSize.Value.Height;
				var widthRatio = hostContentActualWidth / TargetSize.Value.Width;

				var ratio = 1.0;

				if ( heightRatio < widthRatio && heightRatio < 1 )
				{
					ratio = heightRatio;
				}
				else if ( widthRatio < 1 )
				{
					ratio = widthRatio;
				}
				AssociatedObject.RenderTransform = new ScaleTransform { ScaleY = ratio, ScaleX = ratio };
				AssociatedObject.RenderTransformOrigin = new Point( .5, .5 );
			}
		}
	}
}