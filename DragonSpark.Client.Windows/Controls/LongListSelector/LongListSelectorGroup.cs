// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace DragonSpark.Client.Windows.Controls.LongListSelector
{
	/// <summary>
	///     Partial definition of LongListSelector. Includes group view code.
	/// </summary>
	public partial class LongListSelector
	{
		Border border;
		LongListSelectorItemsControl itemsControl;
		Popup groupSelectorPopup;

		void OpenPopup()
		{
			SaveSystemState( false, false );
			BuildPopup();
			AttachToPageEvents();
			groupSelectorPopup.IsOpen = true;

			// This has to happen eventually anyway, and this forces the ItemsControl to 
			// expand it's template, populate it's items etc.
			UpdateLayout();
		}

		void popup_Opened( object sender, EventArgs e )
		{
			SafeRaise.Raise( GroupViewOpened, this, () => new GroupViewOpenedEventArgs( itemsControl ) );
		}

		/// <summary>
		///     Closes the group popup.
		/// </summary>
		/// <param name="selectedGroup">The selected group.</param>
		/// <param name="raiseEvent">Should the GroupPopupClosing event be raised.</param>
		/// <returns>True if the event was not raised or if it was raised and e.Handled is false.</returns>
		bool ClosePopup( object selectedGroup, bool raiseEvent )
		{
			if ( raiseEvent )
			{
				GroupViewClosingEventArgs args = null;

				SafeRaise.Raise( GroupViewClosing, this, () => { return args = new GroupViewClosingEventArgs( itemsControl, selectedGroup ); } );

				if ( args != null && args.Cancel )
				{
					return false;
				}
			}

			if ( groupSelectorPopup != null )
			{
				RestoreSystemState();
				groupSelectorPopup.IsOpen = false;
				DetachFromPageEvents();
				groupSelectorPopup.Child = null;
				border = null;
				itemsControl = null;
				groupSelectorPopup = null;
			}

			return true;
		}

		void BuildPopup()
		{
			groupSelectorPopup = new Popup();
			groupSelectorPopup.Opened += popup_Opened;

			// Support the background color jumping through. Note that the 
			// alpha channel will be ignored, unless it is a purely transparent
			// value (such as when a user uses Transparent as the background
			// on the control).
			var background = Background as SolidColorBrush;
			var bg = (Color)Resources["PhoneBackgroundColor"];
			if ( background != null
				 && background.Color != null
				 && background.Color.A > 0 )
			{
				bg = background.Color;
			}
			border = new Border
			{
				Background = new SolidColorBrush(
					Color.FromArgb( 0xa0, bg.R, bg.G, bg.B ) )
			};

			// Prevents touch events from bubbling up for most handlers.
			border.ManipulationStarted += ( ( o, e ) => e.Handled = true );
			border.ManipulationCompleted += ( ( o, e ) => e.Handled = true );
			border.ManipulationDelta += ( ( o, e ) => e.Handled = true );

			var gestureHandler = new MouseButtonEventHandler( ( o, e ) => e.Handled = true );
			border.MouseLeftButtonUp += gestureHandler;
			// _border.DoubleTap += gestureHandler;
			border.MouseRightButtonUp += gestureHandler;

			itemsControl = new LongListSelectorItemsControl { ItemTemplate = GroupItemTemplate, ItemsPanel = GroupItemsPanel, ItemsSource = ItemsSource };

			itemsControl.GroupSelected += itemsControl_GroupSelected;

			groupSelectorPopup.Child = border;
			var sv = new ScrollViewer { HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled };

			itemsControl.HorizontalAlignment = HorizontalAlignment.Center;
			itemsControl.Margin = new Thickness( 0, 12, 0, 0 );
			border.Child = sv;
			sv.Content = itemsControl;

			SetItemsControlSize();
		}

		void SetItemsControlSize()
		{
			/*Rect client = GetTransformedRect();
			if (_border != null)
			{
				// _border.RenderTransform = GetTransform();

				_border.Width = client.Width;
				_border.Height = client.Height;
			}*/
		}

		void itemsControl_GroupSelected( object sender, GroupSelectedEventArgs e )
		{
			if ( ClosePopup( e.Group, true ) )
			{
				ScrollToGroup( e.Group );
			}
		}

		void AttachToPageEvents()
		{
			/*NavigationWindow frame = Application.Current.MainWindow as NavigationWindow;
			if (frame != null)
			{
				_page = frame.Content as PhoneApplicationPage;
				if (_page != null)
				{
					_page.BackKeyPress += page_BackKeyPress;
					_page.OrientationChanged += page_OrientationChanged;
				}
			}*/
		}

		void DetachFromPageEvents()
		{
			/*if (_page != null)
			{
				_page.BackKeyPress -= page_BackKeyPress;
				_page.OrientationChanged -= page_OrientationChanged;
				_page = null;
			}*/
		}

		void page_BackKeyPress( object sender, CancelEventArgs e )
		{
			e.Cancel = true;
			ClosePopup( null, true );
		}

		/*void page_OrientationChanged(object sender, OrientationChangedEventArgs e)
		{
			SetItemsControlSize();
		}*/

		/*private static Rect GetTransformedRect()
		{
			bool isLandscape = IsLandscape(GetPageOrientation());

			return new Rect(0, 0,
				isLandscape ? _screenHeight : _screenWidth,
				isLandscape ? _screenWidth : _screenHeight);
		}*/

		/*private static Transform GetTransform()
		{
			PageOrientation orientation = GetPageOrientation();

			switch (orientation)
			{
				case PageOrientation.LandscapeLeft:
				case PageOrientation.Landscape:
					return new CompositeTransform() { Rotation = 90, TranslateX = _screenWidth };
				case PageOrientation.LandscapeRight:
					return new CompositeTransform() { Rotation = -90, TranslateY = _screenHeight };
				default:
					return null;
			}
		}*/

		/*private static bool IsLandscape(PageOrientation orientation)
		{
			return orientation == PageOrientation.Landscape || orientation == PageOrientation.LandscapeLeft || orientation == PageOrientation.LandscapeRight;
		}

		private static PageOrientation GetPageOrientation()
		{
			PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (frame != null)
			{
				PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
				
				if (page != null)
				{
					return page.Orientation;
				}
			}

			return PageOrientation.None;
		}*/

		void SaveSystemState( bool newSystemTrayVisible, bool newApplicationBarVisible )
		{
			/*_systemTrayVisible = SystemTray.IsVisible;
			SystemTray.IsVisible = newSystemTrayVisible;

			PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (frame != null)
			{
				PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
				if (page != null && page.ApplicationBar != null)
				{
					_applicationBarVisible = page.ApplicationBar.IsVisible;
					page.ApplicationBar.IsVisible = newApplicationBarVisible;
				}
			}*/
		}

		void RestoreSystemState()
		{
			/*SystemTray.IsVisible = _systemTrayVisible;

			PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (frame != null)
			{
				PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
				if (page != null && page.ApplicationBar != null)
				{
					page.ApplicationBar.IsVisible = _applicationBarVisible;
				}
			}*/
		}
	}
}
