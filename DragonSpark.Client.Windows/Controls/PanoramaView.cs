using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DragonSpark.Client.Windows.Controls
{
	enum Snapping
	{
		SnapLeft = 0,
		SnapRight
	}

	/// <summary>
	///     Helper class for PanoramaControl.
	///     Implements the UI logic.
	/// </summary>
	class PanoramaView
	{
		const string BackgroundPanelName = "BackgroundPanel";
		const string TitlePanelName = "TitlePanel";
		const string ItemsPanelName = "ItemsPanel";
		const string BackgroundPanelHostName = "BackgroundPanelHost";
		const string TitlePanelHostName = "TitlePanelHost";
		const string ItemsPanelHostName = "ItemsPanelHost";

		readonly Panel layoutRoot;
		readonly PanoramaControl parent;
		readonly ItemCollection items;

		ScrollHost backgroundHost;
		ScrollHost titleHost;
		ScrollHost itemsHost;

		const double AnimationDuration = 1000.0;
		Storyboard storyboard;
		public event ScrollCompletedEventHandler ScrollCompleted;

		bool ready;

		public PanoramaView( PanoramaControl parent )
		{
			this.parent = parent;
			layoutRoot = parent.LayoutRoot;
			items = this.parent.Items;

			backgroundHost.Transform = new TranslateTransform();
			titleHost.Transform = new TranslateTransform();
			itemsHost.Transform = new TranslateTransform();
		}

		/// <summary>
		///     Initialize the panel hosting control presenters to duplicate (creating a virtual carousel)
		/// </summary>
		/// <param name="panel">Carousel panel</param>
		/// <param name="left">Control to add to the left</param>
		/// <param name="right">Control to add to the right</param>
		/// <param name="pad">Number of empty padding pixels to add to 'left' and 'right'</param>
		void InitializeHost( Panel panel, FrameworkElement left, FrameworkElement right, double pad )
		{
			// reset/initialize layout with dummy values
			if ( panel.Children.Count == 1 )
			{
				panel.Children.Insert( 0, new Rectangle() );
				panel.Children.Add( new Rectangle() );
			}
			panel.SetValue( Canvas.LeftProperty, 0.0 );

			// insert items ?
			if ( items.Count > 0 )
			{
				// duplicate left
				var source = left.Rendered( new Size( (int)( left.ActualWidth + pad ), (int)left.ActualHeight ) );
				double offset = source.PixelWidth;
				panel.Children[0] = new Image { Source = source, CacheMode = new BitmapCache() };
				
				// duplicate right
				panel.Children[2] = new Image { Source = right.Rendered( new Size( (int)( right.ActualWidth + pad ), (int)right.ActualHeight ), new TranslateTransform { X = pad } ), CacheMode = new BitmapCache() };

				// adjust panel position
				panel.SetValue( Canvas.LeftProperty, -offset );
			}
		}

		public void Initialize()
		{
			if ( !ready )
			{
				// fetch template elements
				var BackgroundPanel = layoutRoot.FindName( BackgroundPanelName ) as ContentPresenter;
				var TitlePanel = layoutRoot.FindName( TitlePanelName ) as ContentPresenter;
				var ItemsPanel = layoutRoot.FindName( ItemsPanelName ) as ItemsPresenter;
				var BackgroundPanelHost = layoutRoot.FindName( BackgroundPanelHostName ) as Panel;
				var TitlePanelHost = layoutRoot.FindName( TitlePanelHostName ) as Panel;
				var ItemsPanelHost = layoutRoot.FindName( ItemsPanelHostName ) as Panel;
				if ( null == BackgroundPanel )
				{
					throw new ArgumentException( string.Format( "Cannot find {0}.", BackgroundPanelName ) );
				}
				if ( null == TitlePanel )
				{
					throw new ArgumentException( string.Format( "Cannot find {0}.", TitlePanelName ) );
				}
				if ( null == ItemsPanel )
				{
					throw new ArgumentException( string.Format( "Cannot find {0}.", ItemsPanelName ) );
				}
				if ( null == BackgroundPanelHost )
				{
					throw new ArgumentException( string.Format( "Cannot find {0}.", BackgroundPanelHostName ) );
				}
				if ( null == TitlePanelHost )
				{
					throw new ArgumentException( string.Format( "Cannot find {0}.", TitlePanelHostName ) );
				}
				if ( null == ItemsPanelHost )
				{
					throw new ArgumentException( string.Format( "Cannot find {0}.", ItemsPanelHostName ) );
				}

				// reset panelhosts
				backgroundHost.Reset();
				titleHost.Reset();
				itemsHost.Reset();

				// create transforms
				BackgroundPanelHost.RenderTransform = backgroundHost.Transform;
				TitlePanelHost.RenderTransform = titleHost.Transform;
				ItemsPanelHost.RenderTransform = itemsHost.Transform;

				// fetch items details
				FrameworkElement item0 = null;
				FrameworkElement itemN = null;
				if ( items.Count > 0 )
				{
					var index0 = 0;
					var indexN = items.Count - 1;
					item0 = items[index0] as FrameworkElement;
					itemN = items[indexN] as FrameworkElement;
				}

				// reset panelhosts layout
				titleHost.Padding = layoutRoot.ActualWidth;
				InitializeHost( BackgroundPanelHost, BackgroundPanel, BackgroundPanel, backgroundHost.Padding );
				InitializeHost( TitlePanelHost, TitlePanel, TitlePanel, titleHost.Padding );
				InitializeHost( ItemsPanelHost, itemN, item0, itemsHost.Padding );

				if ( items.Count > 0 )
				{
					var maxN = Math.Max( itemN.Width, layoutRoot.ActualWidth );

					// panelhosts width
					backgroundHost.Width = BackgroundPanel.ActualWidth;
					titleHost.Width = TitlePanel.ActualWidth;
					itemsHost.Width = items.GetTotalWidth() - itemN.Width + maxN;

					// panelhosts speed
					titleHost.Speed = backgroundHost.Width / itemsHost.Width;
					titleHost.Speed = titleHost.Width / itemsHost.Width;
					if ( itemsHost.Width > maxN )
					{
						backgroundHost.Speed = ( backgroundHost.Width - maxN ) / ( itemsHost.Width - maxN );
					}
				}

				// done
				ready = true;
			}
		}

		public void Invalidate( bool lazy = true )
		{
			ready = false;

			// reset now ?
			if ( !lazy )
			{
				Initialize();
			}
		}

		public void MoveTo( int index )
		{
			Initialize();

			// nothing to do
			if ( items.Count == 0 )
			{
				return;
			}

			// move to new position
			Position = items.GetItemPosition( index );
		}

		public void ScrollTo( int index, Snapping snap = Snapping.SnapLeft )
		{
			Initialize();

			// nothing to do
			if ( items.Count == 0 )
			{
				return;
			}

			// animate to new position
			ScrollStart( index, snap, AnimationDuration );
		}

		public void ScrollSkip()
		{
			Initialize();

			// nothing to do
			if ( items.Count == 0 )
			{
				return;
			}

			// storyboard not completed yet
			// speed it up
			if ( ( null != storyboard ) &&
				 ( storyboard.GetCurrentState() == ClockState.Active ) )
			{
				storyboard.SkipToFill();
				Storyboard_Completed( storyboard, new EventArgs() );
				storyboard = null;
			}
		}

		public double Position
		{
			get
			{
				Initialize();

				return -itemsHost.Transform.X / itemsHost.Speed;
			}
			set
			{
				Initialize();

				// nothing to do
				if ( items.Count == 0 )
				{
					return;
				}

				// complete current animation
				ScrollSkip();

				// adjust transforms
				backgroundHost.Transform.X = -value * backgroundHost.Speed;
				titleHost.Transform.X = -value * titleHost.Speed;
				itemsHost.Transform.X = -value * itemsHost.Speed;
			}
		}

		void ScrollStart( int index, Snapping snap, double milliseconds = 0 )
		{
			Initialize();

			// positions
			var offsetBackground = 0.0;
			var offsetTitle = 0.0;
			var offsetItems = 0.0;

			//
			// adjust destination item positions
			//
			var index0 = 0;
			var indexN = items.Count - 1;

			// scroll from first to last
			if ( index < index0 )
			{
				if ( snap == Snapping.SnapLeft )
				{
					offsetItems = items.GetItemPosition( index0 ) - items.GetItemWidth( indexN );
				}
				else
				{
					offsetItems = items.GetItemPosition( index0 ) - parent.DefaultItemWidth;
				}
			}
			// scroll from last to first
			else if ( index > indexN )
			{
				// since we're moving left to right, and only a item at a time
				// we can only snap to left here...
				offsetItems = items.GetItemPosition( indexN ) + items.GetItemWidth( indexN );
			}
			// normal scroll
			else
			{
				if ( snap == Snapping.SnapLeft )
				{
					offsetItems = items.GetItemPosition( index );
				}
				else
				{
					offsetItems = items.GetItemPosition( index ) + items.GetItemWidth( index ) - parent.DefaultItemWidth;
				}
			}

			//
			// adjust animation speed
			//
			var offset = Math.Abs( offsetItems - Position );
			if ( offset < layoutRoot.ActualWidth )
			{
				milliseconds *= offset / layoutRoot.ActualWidth;
			}

			//
			// adjust positions
			//
			offsetBackground = offsetItems * backgroundHost.Speed;
			offsetTitle = offsetItems * titleHost.Speed;
			offsetItems = offsetItems * itemsHost.Speed;

			// back to last
			if ( offsetItems < 0 )
			{
				offsetBackground = items.GetLastItemPosition() - itemsHost.Width;
				offsetTitle = offsetBackground * titleHost.Speed - titleHost.Padding;
				if ( items.Count == 1 )
				{
					// only 1 item : scroll the entire background
					offsetBackground = -backgroundHost.Width;
				}
			}
			// back to first
			else if ( offsetItems >= items.GetTotalWidth() )
			{
				offsetBackground = backgroundHost.Width;
				offsetTitle = titleHost.Width + titleHost.Padding;
			}

			//
			// start storyboard
			//
			storyboard = new Storyboard();
			storyboard.Completed += Storyboard_Completed;
			storyboard.Children.Add( CreateAnimation( backgroundHost.Transform, TranslateTransform.XProperty, -offsetBackground, milliseconds ) );
			storyboard.Children.Add( CreateAnimation( titleHost.Transform, TranslateTransform.XProperty, -offsetTitle, milliseconds ) );
			storyboard.Children.Add( CreateAnimation( itemsHost.Transform, TranslateTransform.XProperty, -offsetItems, milliseconds ) );
			storyboard.Begin();
		}

		public DoubleAnimation CreateAnimation( DependencyObject obj, DependencyProperty prop, double value, double milliseconds, EasingMode easing = EasingMode.EaseOut )
		{
			var ease = new CubicEase { EasingMode = easing };
			var animation = new DoubleAnimation
			{
				Duration = new Duration( TimeSpan.FromMilliseconds( milliseconds ) ),
				From = Convert.ToDouble( obj.GetValue( prop ) ),
				To = Convert.ToDouble( value ),
				FillBehavior = FillBehavior.HoldEnd,
				EasingFunction = ease
			};
			Storyboard.SetTarget( animation, obj );
			Storyboard.SetTargetProperty( animation, new PropertyPath( prop ) );

			return animation;
		}

		void Storyboard_Completed( object sender, EventArgs e )
		{
			var sb = sender as Storyboard;
			if ( null != sb )
			{
				sb.Completed -= Storyboard_Completed;
			}

			// find selected item
			var index = items.GetIndexOfPosition( Position );

			// raise event for any listener out there
			if ( null != ScrollCompleted )
			{
				ScrollCompleted( this, new ScrollCompletedEventArgs { SelectedIndex = index } );
			}
		}
	}
}
