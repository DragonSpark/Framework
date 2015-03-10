using DragonSpark.Application.Client.Controls.LongListSelector;
using DragonSpark.Extensions;
using FirstFloor.ModernUI.Windows.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xamarin.Forms;
using Xceed.Wpf.Toolkit.Zoombox;
using Binding = System.Windows.Data.Binding;
using DataTemplate = System.Windows.DataTemplate;
using ListView = Xamarin.Forms.ListView;
using Point = System.Windows.Point;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class ListViewRenderer : ViewRenderer<ListView, LongListSelector>
	{
		/*class Animatable : IAnimatable
		{
			public void BatchBegin()
			{}

			public void BatchCommit()
			{}
		}
*/
		Animatable animatable;
		FixedLongListSelector listBox;
		private System.Windows.Controls.ProgressBar progressBar;
		Zoombox viewport;
		object fromNative;
		bool itemNeedsSelecting;
		readonly List<Tuple<FrameworkElement, Binding, Brush>> previousHighlights = new List<Tuple<FrameworkElement, Binding, Brush>>();
		public static readonly DependencyProperty HighlightWhenSelectedProperty = DependencyProperty.RegisterAttached( "HighlightWhenSelected", typeof(bool), typeof(ListViewRenderer), new PropertyMetadata( false ) );

		protected override void OnElementChanged( ElementChangedEventArgs<ListView> e )
		{
			base.OnElementChanged( e );
			Element.PropertyChanged += OnElementPropertyChanged;
			Element.ScrollToRequested += OnScrollToRequested;
			if ( Element.SelectedItem != null )
			{
				itemNeedsSelecting = true;
			}
			listBox = new FixedLongListSelector
			{
				DataContext = Element,
				ItemsSource = Element.TemplatedItems,
				ItemTemplate = (DataTemplate)System.Windows.Application.Current.Resources["CellTemplate"],
				/*JumpListStyle = (System.Windows.Style)System.Windows.Application.Current.Resources["HeaderJumpStyle"],*/
				ListHeaderTemplate = (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["View"],
				ListFooterTemplate = (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["View"]
				/*GroupHeaderTemplate = (DataTemplate)Application.Current.Resources["ListViewHeader"],
				JumpListStyle = (Style)Application.Current.Resources["HeaderJumpStyle"]*/
			};
			// listBox.SetBinding( LongListSelector.IsGroupingEnabledProperty, new Binding( "IsGroupingEnabled" ) );
			listBox.SelectionChanged += OnNativeSelectionChanged;
			listBox.MouseLeftButtonUp += OnNativeItemTapped;
			listBox.Link += OnItemRealized;
			this.listBox.PullToRefreshStarted += new EventHandler(this.OnPullToRefreshStarted);
			this.listBox.PullToRefreshCompleted += new EventHandler(this.OnPullToRefreshCompleted);
			this.listBox.PullToRefreshCanceled += new EventHandler(this.OnPullToRefreshCanceled);
			this.listBox.PullToRefreshStatusUpdated += new EventHandler(this.OnPullToRefreshStatusUpdated);

			SetNativeControl( listBox );
			this.progressBar = new System.Windows.Controls.ProgressBar
			{
				Maximum = 1.0,
				Visibility = Visibility.Collapsed
			};
			base.Children.Add(this.progressBar);
			this.UpdateHeader();
			this.UpdateFooter();
		}
		private void OnPullToRefreshStatusUpdated(object sender, EventArgs eventArgs)
		{
			if (!base.Element.IsPullToRefreshEnabled)
			{
				return;
			}
			this.progressBar.Value = Math.Max(0.0, Math.Min(1.0, this.listBox.PullToRefreshStatus));
		}
		private void OnPullToRefreshCanceled(object sender, EventArgs args)
		{
			if (!base.Element.IsPullToRefreshEnabled)
			{
				return;
			}
			this.progressBar.Visibility = Visibility.Collapsed;
		}
		private void OnPullToRefreshCompleted(object sender, EventArgs args)
		{
			if (!base.Element.IsPullToRefreshEnabled)
			{
				return;
			}
			this.progressBar.IsIndeterminate = true;
			((IListViewController)base.Element).SendRefreshing();
		}

		private void OnPullToRefreshStarted(object sender, EventArgs args)
		{
			if (!base.Element.IsPullToRefreshEnabled)
			{
				return;
			}
			this.progressBar.Visibility = Visibility.Visible;
			this.progressBar.IsIndeterminate = false;
			this.progressBar.Value = Math.Max(0.0, Math.Min(1.0, this.listBox.PullToRefreshStatus));
		}
		protected override void UpdateNativeWidget()
		{
			base.UpdateNativeWidget();
			this.progressBar.Width = base.Element.Width;
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if ( e.PropertyName == ListView.SelectedItemProperty.PropertyName )
			{
				if (e.PropertyName == ListView.SelectedItemProperty.PropertyName)
				{
					this.OnItemSelected(base.Element.SelectedItem);
					return;
				}
				if (e.PropertyName == "HeaderElement")
				{
					this.UpdateHeader();
					return;
				}
				if (e.PropertyName == "FooterElement")
				{
					this.UpdateFooter();
					return;
				}
				if (e.PropertyName == ListView.IsRefreshingProperty.PropertyName)
				{
					this.UpdateIsRefreshing();
				}
			}
		}

		private void UpdateIsRefreshing()
		{
			if (base.Element.IsRefreshing)
			{
				this.progressBar.Visibility = Visibility.Visible;
				this.progressBar.IsIndeterminate = true;
				return;
			}
			this.progressBar.IsIndeterminate = false;
			this.progressBar.Visibility = ((this.listBox.IsInPullToRefresh && base.Element.IsPullToRefreshEnabled) ? Visibility.Visible : Visibility.Collapsed);
		}

		double GetHeight(Dictionary<System.Windows.DataTemplate, FrameworkElement> reusables, System.Windows.DataTemplate template, object bindingContext)
		{
			double actualWidth = base.Control.ActualWidth;
			FrameworkElement frameworkElement;
			if (!reusables.TryGetValue(template, out frameworkElement))
			{
				frameworkElement = (FrameworkElement)template.LoadContent();
				frameworkElement.DataContext = bindingContext;
				frameworkElement.Measure(new System.Windows.Size(actualWidth, double.PositiveInfinity));
				frameworkElement.DataContext = null;
				frameworkElement.Measure(new System.Windows.Size(actualWidth, double.PositiveInfinity));
				Control control = frameworkElement as Control;
				if (control != null)
				{
					control.FontFamily = base.Control.FontFamily;
				}
				reusables[template] = frameworkElement;
			}
			frameworkElement.DataContext = bindingContext;
			frameworkElement.Measure(new System.Windows.Size(actualWidth, double.PositiveInfinity));
			return frameworkElement.DesiredSize.Height;
		}

		private void UpdateHeader()
		{
			base.Control.ListHeader = ((IListViewController)base.Element).HeaderElement;
		}
		private void UpdateFooter()
		{
			base.Control.ListFooter = ((IListViewController)base.Element).FooterElement;
		}

		void OnScrollToRequested( object sender, ScrollToRequestedEventArgs e )
		{
			if ( animatable == null && e.ShouldAnimate )
			{
				animatable = new Animatable();
			}
			if ( viewport == null )
			{
				if ( VisualTreeHelper.GetChildrenCount( listBox ) == 0 )
				{
					RoutedEventHandler handler = null;
					handler = delegate
					{
						this.Control.Loaded -= handler;
						this.OnScrollToRequested( sender, e );
					};
					Control.Loaded += handler;
					return;
				}
				viewport = (Zoombox)VisualTreeHelper.GetChild( VisualTreeHelper.GetChild( VisualTreeHelper.GetChild( listBox, 0 ), 0 ), 0 );
				if ( viewport.Viewport.Bottom == 0.0 )
				{
					ZoomboxViewChangedEventHandler viewportChanged = null;
					viewportChanged = ( o, args ) =>
					{
						if ( viewport.Viewport.Bottom != 0.0 )
						{
							viewport.CurrentViewChanged -= viewportChanged;
							OnScrollToRequested( sender, e );
						}
					};
					viewport.CurrentViewChanged += viewportChanged;
					return;
				}
			}
			var num = 0.0;
			var num2 = 0.0;
			var num3 = 0.0;
			var reusables = new Dictionary<DataTemplate, FrameworkElement>();
			var flag = false;

			// TODO: Enable grouping.
			/*if (base.Element.IsGroupingEnabled)
			{
				for (int i = 0; i < base.Element.TemplatedItems.Count; i++)
				{
					if (flag)
					{
						break;
					}
					TemplatedItemsList<ItemsView<Cell>, Cell> group = base.Element.TemplatedItems.GetGroup(i);
					double height = this.GetHeight(reusables, base.Control.GroupHeaderTemplate, group);
					num += height;
					for (int j = 0; j < group.Count; j++)
					{
						Cell cell = group[j];
						double height2 = this.GetHeight(reusables, base.Control.ItemTemplate, cell);
						if ((object.ReferenceEquals(group.BindingContext, e.Group) || e.Group == null) && object.ReferenceEquals(cell.BindingContext, e.Item))
						{
							num3 = height;
							num2 = height2;
							flag = true;
							break;
						}
						num += height2;
					}
				}
			}
			else
			{
			*/
				for (int k = 0; k < base.Element.TemplatedItems.Count; k++)
				{
					Cell cell2 = base.Element.TemplatedItems[k];
					double height3 = this.GetHeight(reusables, base.Control.ItemTemplate, cell2);
					if (object.ReferenceEquals(cell2.BindingContext, e.Item))
					{
						flag = true;
						num2 = height3;
						break;
					}
					num += height3;
				}
			// }
			if ( !flag )
			{
				return;
			}
			var scrollToPosition = e.Position;
			if ( scrollToPosition == ScrollToPosition.MakeVisible )
			{
				if ( num >= viewport.Viewport.Top && num <= viewport.Viewport.Bottom )
				{
					return;
				}
				if ( num > viewport.Viewport.Bottom )
				{
					scrollToPosition = ScrollToPosition.End;
				}
				else
				{
					scrollToPosition = ScrollToPosition.Start;
				}
			}
			if ( scrollToPosition == ScrollToPosition.Start && Element.IsGroupingEnabled )
			{
				num -= num3;
			}
			else
			{
				if ( scrollToPosition == ScrollToPosition.Center )
				{
					num -= viewport.ActualHeight / 2.0 + num2 / 2.0;
				}
				else
				{
					if ( scrollToPosition == ScrollToPosition.End )
					{
						num = num - viewport.ActualHeight + num2;
					}
				}
			}
			var startY = viewport.Viewport.Y;
			var distance = num - startY;
			if ( e.ShouldAnimate )
			{
				var animation = new Animation( v => viewport.ZoomOrigin = new Point( 0.0, startY + distance * v ) );
				animation.Commit( animatable, "ScrollTo", 16u, 500u, Easing.CubicInOut, null, null );
				return;
			}
			viewport.ZoomOrigin = new Point( 0.0, num );
		}

		public override SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint )
		{
			var desiredSize = base.GetDesiredSize( widthConstraint, heightConstraint );
			desiredSize.Minimum = new global::Xamarin.Forms.Size( 40.0, 40.0 );
			return desiredSize;
		}

		void OnItemRealized( object sender, LinkUnlinkEventArgs linkUnlinkEventArgs )
		{
			if ( itemNeedsSelecting )
			{
				linkUnlinkEventArgs.ContentPresenter.DataContext.As<Cell>( cell =>
				{
					if ( cell != null && Equals( cell.BindingContext, Element.SelectedItem ) )
					{
						itemNeedsSelecting = false;
						OnItemSelected( Element.SelectedItem );
					}
				} );
			}
		}

		public static bool GetHighlightWhenSelected( DependencyObject dependencyObject )
		{
			return (bool)dependencyObject.GetValue( HighlightWhenSelectedProperty );
		}

		public static void SetHighlightWhenSelected( DependencyObject dependencyObject, bool value )
		{
			dependencyObject.SetValue( HighlightWhenSelectedProperty, value );
		}

		IEnumerable<FrameworkElement> FindHighlight( FrameworkElement element )
		{
			var frameworkElement = element;
			do
			{
				element = frameworkElement;
				if ( element is CellControl )
				{
					goto IL_1E;
				}
				frameworkElement = ( element.GetParent() as FrameworkElement );
			}
			while ( frameworkElement != null );
			frameworkElement = element;
			IL_1E:
			return FindHighlightCore( frameworkElement );
		}

		IEnumerable<FrameworkElement> FindHighlightCore( DependencyObject element )
		{
			var childrenCount = VisualTreeHelper.GetChildrenCount( element );
			for ( var i = 0; i < childrenCount; i++ )
			{
				var child = VisualTreeHelper.GetChild( element, i );
				var labelRenderer = child as LabelRenderer;
				var frameworkElement = child as FrameworkElement;
				if ( frameworkElement != null && ( GetHighlightWhenSelected( frameworkElement ) || labelRenderer != null ) )
				{
					if ( labelRenderer != null )
					{
						yield return labelRenderer.Control;
					}
					else
					{
						yield return frameworkElement;
					}
				}
				foreach ( var current in FindHighlightCore( frameworkElement ) )
				{
					yield return current;
				}
			}
		}

		void OnNativeItemTapped( object sender, EventArgs e )
		{
			var cell = (Cell)Control.SelectedItem;
			if ( cell == null )
			{
				return;
			}
			Cell item = null;
			if ( Element.IsGroupingEnabled )
			{
				var group = TemplatedItemsList<ItemsView<Cell>, Cell>.GetGroup( cell );
				item = group.HeaderContent;
			}
			fromNative = cell.BindingContext;
			if ( Element.IsGroupingEnabled )
			{
				Element.NotifyRowTapped( TemplatedItemsList<ItemsView<Cell>, Cell>.GetIndex( item ), TemplatedItemsList<ItemsView<Cell>, Cell>.GetIndex( cell ) );
				return;
			}
			Element.NotifyRowTapped( TemplatedItemsList<ItemsView<Cell>, Cell>.GetIndex( cell ) );
		}

		void OnNativeSelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			if ( e.AddedItems.Count == 0 )
			{
				return;
			}
			var cell = (Cell)e.AddedItems[0];
			if ( cell == null )
			{
				RestorePreviousSelectedVisual();
				return;
			}
			RestorePreviousSelectedVisual();
			var frameworkElement = FindElement( cell );
			if ( frameworkElement != null )
			{
				SetSelectedVisual( frameworkElement );
			}
		}

		FrameworkElement FindElement( Cell cell )
		{
			return FindDescendants<CellControl>( listBox ).FirstOrDefault( current => ReferenceEquals( cell, current.DataContext ) );
		}

		void RestorePreviousSelectedVisual()
		{
			foreach ( var current in previousHighlights )
			{
				if ( current.Item2 != null )
				{
					current.Item1.SetForeground( current.Item2 );
				}
				else
				{
					current.Item1.SetForeground( current.Item3 );
				}
			}
			previousHighlights.Clear();
		}

		void SetSelectedVisual( FrameworkElement element )
		{
			var enumerable = FindHighlight( element );
			foreach ( var current in enumerable )
			{
				Brush item = null;
				var foregroundBinding = current.GetForegroundBinding();
				if ( foregroundBinding == null )
				{
					item = current.GetForeground();
				}
				previousHighlights.Add( new Tuple<FrameworkElement, Binding, Brush>( current, foregroundBinding, item ) );
				current.SetForeground( (Brush)System.Windows.Application.Current.Resources["PhoneAccentBrush"] );
			}
		}

		Tuple<Cell, Cell> FindCellAndParent( MouseEventArgs e, out FrameworkElement element )
		{
			Cell cell = null;
			Cell cell2 = null;
			element = ( e.OriginalSource as FrameworkElement );
			if ( element != null )
			{
				cell = ( element.DataContext as Cell );
			}
			if ( cell == null )
			{
				cell = FindCell( e, out element );
			}
			if ( cell == null || TemplatedItemsList<ItemsView<Cell>, Cell>.GetIsGroupHeader( cell ) )
			{
				return new Tuple<Cell, Cell>( null, cell );
			}
			var actualHeight = element.ActualHeight;
			var num = e.GetPosition( listBox ).Y;
			while ( cell2 == null )
			{
				num -= actualHeight;
				var intersectingRect = new Rect( 0.0, num, listBox.ActualWidth, element.ActualHeight );
				IEnumerable<UIElement> enumerable = listBox.FindElementsAt<FrameworkElement>( intersectingRect ).ToArray();
				if ( !enumerable.Any() )
				{
					return new Tuple<Cell, Cell>( null, null );
				}
				foreach ( var cell3 in enumerable.OfType<FrameworkElement>().Select( current => current.DataContext as Cell ).Where( cell3 => cell3 != null && TemplatedItemsList<ItemsView<Cell>, Cell>.GetIsGroupHeader( cell3 ) ) )
				{
					cell2 = cell3;
					break;
				}
			}
			return new Tuple<Cell, Cell>( cell, cell2 );
		}

		Cell FindCell( MouseEventArgs e, out FrameworkElement element )
		{
			Cell cell = null;
			element = ( e.OriginalSource as FrameworkElement );
			if ( element != null )
			{
				cell = ( element.DataContext as Cell );
			}
			if ( cell == null )
			{
				var position = e.GetPosition( listBox );
				IEnumerable<UIElement> enumerable = listBox.FindElementsAt<FrameworkElement>( position );
				foreach ( var current in enumerable.OfType<FrameworkElement>().Where( current => ( cell = ( current.DataContext as Cell ) ) != null ) )
				{
					element = current;
					break;
				}
			}
			return cell;
		}

		static IEnumerable<T> FindAncestors<T>( DependencyObject dobj ) where T : DependencyObject
		{
			while ( ( dobj = VisualTreeHelper.GetParent( dobj ) ) != null )
			{
				if ( dobj is T )
				{
					yield return (T)dobj;
				}
			}
		}

		static IEnumerable<T> FindDescendants<T>( DependencyObject dobj ) where T : DependencyObject
		{
			var childrenCount = VisualTreeHelper.GetChildrenCount( dobj );
			for ( var i = 0; i < childrenCount; i++ )
			{
				var child = VisualTreeHelper.GetChild( dobj, i );
				if ( child is T )
				{
					yield return (T)child;
				}
				foreach ( var current in FindDescendants<T>( child ) )
				{
					yield return current;
				}
			}
		}

		void OnItemSelected( object selectedItem )
		{
			if ( fromNative != null && Equals( selectedItem, fromNative ) )
			{
				fromNative = null;
				return;
			}
			RestorePreviousSelectedVisual();
			if ( selectedItem == null )
			{
				listBox.SelectedItem = selectedItem;
				return;
			}
			var enumerable = FindDescendants<CellControl>( listBox );
			var cellControl = enumerable.FirstOrDefault( delegate( CellControl i )
			{
				var cell = (Cell)i.DataContext;
				return Equals( cell.BindingContext, selectedItem );
			} );
			if ( cellControl == null )
			{
				itemNeedsSelecting = true;
				return;
			}
			SetSelectedVisual( cellControl );
		}
	}


	public class FixedLongListSelector : LongListSelector
	{
		private bool isInPullToRefresh;
		private double pullToRefreshStatus;
		private System.Windows.Point lastPosition;
		public event EventHandler PullToRefreshStarted;
		public event EventHandler PullToRefreshCompleted;
		public event EventHandler PullToRefreshCanceled;
		public event EventHandler PullToRefreshStatusUpdated;
		public Zoombox ViewportControl
		{
			get;
			private set;
		}
		public double PullToRefreshStatus
		{
			get
			{
				return this.pullToRefreshStatus;
			}
			set
			{
				if (this.pullToRefreshStatus == value)
				{
					return;
				}
				this.pullToRefreshStatus = value;
				EventHandler pullToRefreshStatusUpdated = this.PullToRefreshStatusUpdated;
				if (pullToRefreshStatusUpdated != null)
				{
					pullToRefreshStatusUpdated(this, EventArgs.Empty);
				}
			}
		}
		public bool IsInPullToRefresh
		{
			get
			{
				return this.isInPullToRefresh;
			}
			private set
			{
				if (this.isInPullToRefresh == value)
				{
					return;
				}
				this.isInPullToRefresh = value;
				if (this.isInPullToRefresh)
				{
					EventHandler pullToRefreshStarted = this.PullToRefreshStarted;
					if (pullToRefreshStarted != null)
					{
						pullToRefreshStarted(this, EventArgs.Empty);
						return;
					}
				}
				else
				{
					EventHandler eventHandler = (this.PullToRefreshStatus >= 1.0) ? this.PullToRefreshCompleted : this.PullToRefreshCanceled;
					if (eventHandler != null)
					{
						eventHandler(this, EventArgs.Empty);
					}
					this.pullToRefreshStatus = 0.0;
				}
			}
		}
		private bool ViewportAtTop
		{
			get { return ViewportControl.Viewport.Top == 0.0; }
		}
		public FixedLongListSelector()
		{
			base.Loaded += OnLoaded;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.ViewportControl != null) // TODO: Implement pull-to-refresh for WPF???
			{
				this.ViewportControl.CurrentViewChanged -= this.OnViewportChanged;
				// this.ViewportControl.ManipulationStateChanged -= new EventHandler<ManipulationStateChangedEventArgs>(this.OnManipulationStateChanged);
			}
			this.ViewportControl = (Zoombox)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 0), 0);
			this.ViewportControl.CurrentViewChanged += OnViewportChanged;
			// this.ViewportControl.ManipulationStateChanged += new EventHandler<ManipulationStateChangedEventArgs>(this.OnManipulationStateChanged);
		}
		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			base.Loaded -= new RoutedEventHandler(this.OnLoaded);
			base.Unloaded += new RoutedEventHandler(this.OnUnloaded);
			Touch.FrameReported += new TouchFrameEventHandler(this.OnFrameReported);
		}
		private void OnFrameReported(object sender, TouchFrameEventArgs e)
		{
			TouchPoint primaryTouchPoint;
			try
			{
				primaryTouchPoint = e.GetPrimaryTouchPoint(this);
			}
			catch (Exception)
			{
				return;
			}
			if (primaryTouchPoint != null && primaryTouchPoint.Action == TouchAction.Move)
			{
				System.Windows.Point position = primaryTouchPoint.Position;
				if (this.IsInPullToRefresh)
				{
					double num = position.Y - this.lastPosition.Y;
					this.PullToRefreshStatus += num / 150.0;
				}
				this.lastPosition = position;
				return;
			}
		}
		private void OnViewportChanged(object o, EventArgs args)
		{
			/*if (this.ViewportControl.ManipulationState == ManipulationState.Manipulating)
			{
				this.IsInPullToRefresh = this.ViewportAtTop;
			}*/
		}
		/*private void OnManipulationStateChanged(object o, ManipulationStateChangedEventArgs args)
		{
			switch (this.ViewportControl.ManipulationState)
			{
			case ManipulationState.Idle:
				this.IsInPullToRefresh = false;
				return;
			case ManipulationState.Manipulating:
				this.IsInPullToRefresh = this.ViewportAtTop;
				return;
			case ManipulationState.Animating:
				this.IsInPullToRefresh = false;
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}*/
		private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			base.Loaded += new RoutedEventHandler(this.OnLoaded);
			base.Unloaded -= new RoutedEventHandler(this.OnUnloaded);
			Touch.FrameReported -= new TouchFrameEventHandler(this.OnFrameReported);
		}
		protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
		{
			System.Windows.Size result;
			try
			{
				result = base.MeasureOverride(availableSize);
			}
			catch (ArgumentException)
			{
				result = base.MeasureOverride(availableSize);
			}
			return result;
		}
	}
}
