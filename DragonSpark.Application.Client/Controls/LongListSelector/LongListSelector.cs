﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DragonSpark.Application.Client.Controls.LongListSelector
{
	/// <summary>
	///     Represents a virtualizing list designed for grouped lists. Can also be
	///     used with flat lists.
	/// </summary>
	/// <QualityBand>Preview</QualityBand>
	[SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "LongListSelector is a complicated control." ), TemplatePart( Name = TemplatedListBoxName, Type = typeof(TemplatedListBox) )]
	public partial class LongListSelector : Control
	{
		#region Constants
		/// <summary>
		///     The templated list box name.
		/// </summary>
		const string TemplatedListBoxName = "TemplatedListBox";

		/// <summary>
		///     This constant is not actively used in the new version, however, the
		///     value is exposed through a deprecated property. For backward-
		///     compatibility only.
		/// </summary>
		const double BufferSizeDefault = 1.0;

		/// <summary>
		///     The Scrolling state name.
		/// </summary>
		const string ScrollingState = "Scrolling";

		/// <summary>
		///     The NotScrolling state name.
		/// </summary>
		const string NotScrollingState = "NotScrolling";

		/// <summary>
		///     The vertical compression top state name.
		/// </summary>
		const string CompressionTop = "CompressionTop";

		/// <summary>
		///     The vertical compression bottom state name.
		/// </summary>
		const string CompressionBottom = "CompressionBottom";

		/// <summary>
		///     The absense of vertical compression state name.
		/// </summary>
		const string NoVerticalCompression = "NoVerticalCompression";

		/// <summary>
		///     The vertical compression state name.
		/// </summary>
		const string VerticalCompressionStateName = "VerticalCompression";

		/// <summary>
		///     The name of the scroll states visual state group.
		/// </summary>
		const string ScrollStatesGroupName = "ScrollStates";
		#endregion

		/// <summary>
		///     Reference to the ListBox hosted in this control.
		/// </summary>
		TemplatedListBox _listBox;

		/// <summary>
		///     Reference to the visual state group for scrolling.
		/// </summary>
		VisualStateGroup _scrollGroup;

		/// <summary>
		///     Reference to the visual state group for  vertical compression.
		/// </summary>
		VisualStateGroup _verticalCompressionGroup;

		/// <summary>
		///     // Used to listen for changes in the ItemsSource
		///     (_rootCollection = ItemsSource as INotifyCollectionChanged).
		/// </summary>
		INotifyCollectionChanged _rootCollection;

		/// <summary>
		///     Used to listen for changes in the groups within ItemsSource.
		/// </summary>
		readonly List<INotifyCollectionChanged> _groupCollections = new List<INotifyCollectionChanged>();

		/// <summary>
		///     Indicates the LLS is in the process of setting the SelectedItem back to its old value.
		/// </summary>
		bool _isResettingSelectedItem;

		#region Properties
		/// <summary>
		///     Gets or sets whether the list is flat instead of a group hierarchy.
		/// </summary>
		public bool IsFlatList { get; set; }

		/// <summary>
		///     Gets or sets whether the list can be (temporarily) scrolled off of the ends.
		/// </summary>
		[SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Backward compatible public setter." ), SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value", Justification = "Backward compatible public setter." ), Obsolete( "IsBouncy is always set to true." )]
		public bool IsBouncy
		{
			get { return true; }
			set { }
		}

		/// <summary>
		///     Gets whether a list header is shown.
		/// </summary>
		bool HasListHeader
		{
			get { return ListHeaderTemplate != null || ListHeader is UIElement; }
		}

		/// <summary>
		///     Gets whether a list footer is shown.
		/// </summary>
		bool HasListFooter
		{
			get { return ListFooterTemplate != null || ListFooter is UIElement; }
		}

		/// <summary>
		///     Gets whether or not the user is manipulating the list, or if an inertial animation is taking place.
		/// </summary>
		public bool IsScrolling { get; private set; }

		/// <summary>
		///     Gets whether or not stretching is taking place.
		/// </summary>
		public bool IsStretching { get; private set; }
		#endregion

		#region Dependency Properties

		#region ItemsSource DependencyProperty
		/// <summary>
		///     The DataSource property. Where all of the items come from.
		/// </summary>
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue( ItemsSourceProperty ); }
			set { SetValue( ItemsSourceProperty, value ); }
		}

		/// <summary>
		///     The DataSource DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register( "ItemsSource", typeof(IEnumerable), typeof(LongListSelector), new PropertyMetadata( null, OnItemsSourceChanged ) );

		static void OnItemsSourceChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			( (LongListSelector)obj ).OnItemsSourceChanged();
		}
		#endregion

		#region SelectedItem DependencyProperty
		/// <summary>
		///     Gets or sets the selected item.
		/// </summary>
		public object SelectedItem
		{
			get { return GetValue( SelectedItemProperty ); }
			set { SetValue( SelectedItemProperty, value ); }
		}

		/// <summary>
		///     The SelectedItem DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register( "SelectedItem", typeof(object), typeof(LongListSelector), new PropertyMetadata( null, OnSelectedItemChanged ) );

		/// <summary>
		///     Called when the selected item changes
		/// </summary>
		static void OnSelectedItemChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			var lls = ( (LongListSelector)obj );
			var shouldFireSelectionChanged = !lls._isResettingSelectedItem;

			// Set the selected item of the ListBox if necessary
			if ( lls._listBox != null )
			{
				var selectedListBoxItem = lls._listBox.SelectedItem as LongListSelectorItem;

				if ( e.NewValue == null )
				{
					lls._listBox.SelectedItem = null;
				}
				else if ( selectedListBoxItem == null || e.NewValue != selectedListBoxItem.Item || selectedListBoxItem.ItemType != LongListSelectorItemType.Item )
				{
					var itemFound = false;

					// Find the corresponding LLS item in the listbox. 
					foreach ( LongListSelectorItem item in lls._listBox.Items )
					{
						if ( item.ItemType == LongListSelectorItemType.Item && item.Item == e.NewValue )
						{
							lls._listBox.SelectedItem = item;
							itemFound = true;
							break;
						}
					}

					// If the item doesn't exist in the list, select null
					if ( !itemFound )
					{
						lls._isResettingSelectedItem = true;
						try
						{
							lls.SelectedItem = null;
						}
						finally
						{
							lls._isResettingSelectedItem = false;
						}
					}
				}
			}

			// Fire the SelectionChanged event
			if ( shouldFireSelectionChanged && e.OldValue != lls.SelectedItem )
			{
				var handler = lls.SelectionChanged;
				if ( handler != null )
				{
					var added = new List<object>();
					var removed = new List<object>();

					if ( e.OldValue != null )
					{
						removed.Add( e.OldValue );
					}

					if ( lls.SelectedItem != null )
					{
						added.Add( lls.SelectedItem );
					}

					handler( obj, new SelectionChangedEventArgs( Selector.SelectionChangedEvent, removed, added ) );
				}
			}
		}
		#endregion

		#region ListHeader DependencyProperty
		/// <summary>
		///     The ListHeader property. Will be used as the first scrollItem in the list.
		/// </summary>
		public object ListHeader
		{
			get { return GetValue( ListHeaderProperty ); }
			set { SetValue( ListHeaderProperty, value ); }
		}

		/// <summary>
		///     The ListHeader DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty ListHeaderProperty =
			DependencyProperty.Register( "ListHeader", typeof(object), typeof(LongListSelector), new PropertyMetadata( null ) );
		#endregion

		#region ListHeaderTemplate DependencyProperty
		/// <summary>
		///     The ListHeaderTemplate provides the template for the ListHeader.
		/// </summary>
		public DataTemplate ListHeaderTemplate
		{
			get { return (DataTemplate)GetValue( ListHeaderTemplateProperty ); }
			set { SetValue( ListHeaderTemplateProperty, value ); }
		}

		/// <summary>
		///     The ListHeaderTemplate DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty ListHeaderTemplateProperty =
			DependencyProperty.Register( "ListHeaderTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata( null, OnDataTemplateChanged ) );
		#endregion

		#region ListFooter DependencyProperty
		/// <summary>
		///     The ListFooter property. Will be used as the first scrollItem in the list.
		/// </summary>
		public object ListFooter
		{
			get { return GetValue( ListFooterProperty ); }
			set { SetValue( ListFooterProperty, value ); }
		}

		/// <summary>
		///     The ListFooter DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty ListFooterProperty =
			DependencyProperty.Register( "ListFooter", typeof(object), typeof(LongListSelector), new PropertyMetadata( null ) );
		#endregion

		#region ListFooterTemplate DependencyProperty
		/// <summary>
		///     The ListFooterTemplate provides the template for the ListFooter.
		/// </summary>
		public DataTemplate ListFooterTemplate
		{
			get { return (DataTemplate)GetValue( ListFooterTemplateProperty ); }
			set { SetValue( ListFooterTemplateProperty, value ); }
		}

		/// <summary>
		///     The ListFooterTemplate DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty ListFooterTemplateProperty =
			DependencyProperty.Register( "ListFooterTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata( null, OnDataTemplateChanged ) );
		#endregion

		#region GroupHeaderTemplate DependencyProperty
		/// <summary>
		///     The GroupHeaderTemplate provides the template for the groups in the items view.
		/// </summary>
		public DataTemplate GroupHeaderTemplate
		{
			get { return (DataTemplate)GetValue( GroupHeaderProperty ); }
			set { SetValue( GroupHeaderProperty, value ); }
		}

		/// <summary>
		///     The GroupHeaderTemplate DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty GroupHeaderProperty =
			DependencyProperty.Register( "GroupHeaderTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata( null, OnDataTemplateChanged ) );
		#endregion

		#region GroupFooterTemplate DependencyProperty
		/// <summary>
		///     The GroupFooterTemplate provides the template for the groups in the items view.
		/// </summary>
		public DataTemplate GroupFooterTemplate
		{
			get { return (DataTemplate)GetValue( GroupFooterProperty ); }
			set { SetValue( GroupFooterProperty, value ); }
		}

		/// <summary>
		///     The GroupFooterTemplate DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty GroupFooterProperty =
			DependencyProperty.Register( "GroupFooterTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata( null, OnDataTemplateChanged ) );
		#endregion

		#region ItemTemplate DependencyProperty
		/// <summary>
		///     The ItemTemplate provides the template for the items in the items view.
		/// </summary>
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue( ItemsTemplateProperty ); }
			set { SetValue( ItemsTemplateProperty, value ); }
		}

		/// <summary>
		///     The ItemTemplate DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty ItemsTemplateProperty =
			DependencyProperty.Register( "ItemTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata( null, OnDataTemplateChanged ) );
		#endregion

		#region DisplayAllGroups DependencyProperty
		/// <summary>
		///     Display all groups whether or not they have items.
		/// </summary>
		public bool DisplayAllGroups
		{
			get { return (bool)GetValue( DisplayAllGroupsProperty ); }
			set { SetValue( DisplayAllGroupsProperty, value ); }
		}

		/// <summary>
		///     DisplayAllGroups DependencyProperty
		/// </summary>
		public static readonly DependencyProperty DisplayAllGroupsProperty =
			DependencyProperty.Register( "DisplayAllGroups", typeof(bool), typeof(LongListSelector), new PropertyMetadata( false, OnDisplayAllGroupsChanged ) );
		#endregion

		#region GroupItemTemplate DependencyProperty
		/// <summary>
		///     The GroupItemTemplate specifies the template that will be used in group view mode.
		/// </summary>
		public DataTemplate GroupItemTemplate
		{
			get { return (DataTemplate)GetValue( GroupItemTemplateProperty ); }
			set { SetValue( GroupItemTemplateProperty, value ); }
		}

		/// <summary>
		///     The GroupItemTemplate DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty GroupItemTemplateProperty =
			DependencyProperty.Register( "GroupItemTemplate", typeof(DataTemplate), typeof(LongListSelector), new PropertyMetadata( null ) );
		#endregion

		#region GroupItemsPanel DependencyProperty
		/// <summary>
		///     The GroupItemsPanel specifies the panel that will be used in group view mode.
		/// </summary>
		public ItemsPanelTemplate GroupItemsPanel
		{
			get { return (ItemsPanelTemplate)GetValue( GroupItemsPanelProperty ); }
			set { SetValue( GroupItemsPanelProperty, value ); }
		}

		/// <summary>
		///     The GroupItemsPanel DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty GroupItemsPanelProperty =
			DependencyProperty.Register( "GroupItemsPanel", typeof(ItemsPanelTemplate), typeof(LongListSelector), new PropertyMetadata( null ) );
		#endregion

		#region BufferSize DependencyProperty
		/// <summary>
		///     The number of "screens" (as defined by the ActualHeight of the LongListSelector) above and below the visible
		///     items of the list that will be filled with items.
		/// </summary>
		[Obsolete( "BufferSize no longer affect items virtualization" )]
		public double BufferSize
		{
			get { return (double)GetValue( BufferSizeProperty ); }
			set { SetValue( BufferSizeProperty, value ); }
		}

		/// <summary>
		///     The BufferSize DependencyProperty
		/// </summary>
		[Obsolete( "BufferSizeProperty no longer affect items virtualization" )] public static readonly DependencyProperty BufferSizeProperty =
			DependencyProperty.Register( "BufferSize", typeof(double), typeof(LongListSelector), new PropertyMetadata( BufferSizeDefault, OnBufferSizeChanged ) );

		[SuppressMessage( "Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly" )]
		static void OnBufferSizeChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			var newValue = (double)e.NewValue;

			if ( newValue < 0 )
			{
				throw new ArgumentOutOfRangeException( "BufferSize" );
			}
		}
		#endregion

		#region MaximumFlickVelocity DependencyProperty
		/// <summary>
		///     The maximum velocity for flicks, in pixels per second.
		/// </summary>
		[Obsolete( "MaximumFlickVelocity is not used anymore." )]
		public double MaximumFlickVelocity
		{
			get { return (double)GetValue( MaximumFlickVelocityProperty ); }
			set { SetValue( MaximumFlickVelocityProperty, value ); }
		}

		/// <summary>
		///     The MaximumFlickVelocity DependencyProperty.
		/// </summary>
		[Obsolete( "MaximumFlickVelocityProperty is not used anymore." )] public static readonly DependencyProperty MaximumFlickVelocityProperty =
			DependencyProperty.Register( "MaximumFlickVelocity", typeof(double), typeof(LongListSelector), new PropertyMetadata( MotionParameters.MaximumSpeed ) );
		#endregion

		#region ShowListHeader DependencyProperty
		/// <summary>
		///     Controls whether or not the ListHeader is shown.
		/// </summary>
		public bool ShowListHeader
		{
			get { return (bool)GetValue( ShowListHeaderProperty ); }
			set { SetValue( ShowListHeaderProperty, value ); }
		}

		/// <summary>
		///     The ShowListHeader DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty ShowListHeaderProperty =
			DependencyProperty.Register( "ShowListHeader", typeof(bool), typeof(LongListSelector), new PropertyMetadata( true, OnShowListHeaderChanged ) );

		static void OnShowListHeaderChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			var control = (LongListSelector)obj;

			if ( control._listBox != null )
			{
				var tuples = (Collection<LongListSelectorItem>)control._listBox.ItemsSource;
				if ( control.ShowListHeader )
				{
					control.AddListHeader( tuples );
				}
				else
				{
					RemoveListHeader( tuples );
				}
			}
		}
		#endregion

		#region ShowListFooter DependencyProperty
		/// <summary>
		///     Controls whether or not the ListFooter is shown.
		/// </summary>
		public bool ShowListFooter
		{
			get { return (bool)GetValue( ShowListFooterProperty ); }
			set { SetValue( ShowListFooterProperty, value ); }
		}

		/// <summary>
		///     The ShowListFooter DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty ShowListFooterProperty =
			DependencyProperty.Register( "ShowListFooter", typeof(bool), typeof(LongListSelector), new PropertyMetadata( true, OnShowListFooterChanged ) );

		static void OnShowListFooterChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			var control = (LongListSelector)obj;

			if ( control._listBox != null )
			{
				var tuples = (Collection<LongListSelectorItem>)control._listBox.ItemsSource;
				if ( control.ShowListFooter )
				{
					control.AddListFooter( tuples );
				}
				else
				{
					RemoveListFooter( tuples );
				}
			}
		}
		#endregion

		#endregion

		#region Events
		/// <summary>
		///     Occurs when the currently selected item changes.
		/// </summary>
		public event SelectionChangedEventHandler SelectionChanged;

		/// <summary>
		///     Occurs when this control starts scrolling.
		/// </summary>
		public event EventHandler ScrollingStarted;

		/// <summary>
		///     Occurs when this control stops scrolling.
		/// </summary>
		public event EventHandler ScrollingCompleted;

		/// <summary>
		///     Occurs when the group Popup's IsOpen has been set to true.
		/// </summary>
		public event EventHandler<GroupViewOpenedEventArgs> GroupViewOpened;

		/// <summary>
		///     Occurs when the group popup is about to close.
		/// </summary>
		public event EventHandler<GroupViewClosingEventArgs> GroupViewClosing;

		/// <summary>
		///     Occurs when an item is about to be "realized".
		/// </summary>
		public event EventHandler<LinkUnlinkEventArgs> Link;

		/// <summary>
		///     Occurs when an item is about to be "un-realized".
		/// </summary>
		public event EventHandler<LinkUnlinkEventArgs> Unlink;

		/// <summary>
		///     Occurs when the user has dragged the items up from the bottom as far as they can go.
		/// </summary>
		public event EventHandler StretchingBottom;

		/// <summary>
		///     Occurs when the user is no longer stretching.
		/// </summary>
		public event EventHandler StretchingCompleted;

		/// <summary>
		///     Occurs when the user has dragged the items down from the top as far as they can go.
		/// </summary>
		public event EventHandler StretchingTop;
		#endregion

		#region Constructor
		/// <summary>
		///     Initializes a new instance of <see cref="LongListSelector" />.
		/// </summary>
		public LongListSelector( /*bool systemTrayVisible, Window page*/ )
		{
			DefaultStyleKey = typeof(LongListSelector);
		}
		#endregion

		//
		// Public methods
		//

		#region ScrollTo(...)
		/// <summary>
		///     Instantly jump to the specified item.
		/// </summary>
		/// <param name="item">Item to jump to.</param>
		public void ScrollTo( object item )
		{
			if ( _listBox != null && item != null )
			{
				var tuples = (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource;
				var lastTuple = tuples[tuples.Count - 1];

				_listBox.ScrollIntoView( lastTuple );

				UpdateLayout();

				foreach ( LongListSelectorItem tuple in _listBox.ItemsSource )
				{
					if ( tuple.Item != null && tuple.Item.Equals( item ) )
					{
						_listBox.ScrollIntoView( tuple );
						break;
					}
				}
			}
		}
		#endregion

		#region ScrollToGroup(...)
		/// <summary>
		///     Instantly jump to the specified  group.
		/// </summary>
		/// <param name="group">Group to jump to.</param>
		public void ScrollToGroup( object group )
		{
			ScrollTo( group );
		}
		#endregion

		#region DisplayGroupView()
		/// <summary>
		///     Invokes the group view if a GroupItemTemplate has been defined.
		/// </summary>
		public void DisplayGroupView()
		{
			if ( GroupItemTemplate != null && !IsFlatList )
			{
				OpenPopup();
			}
		}
		#endregion

		#region CloseGroupView()
		/// <summary>
		///     Closes the group view unconditionally.
		/// </summary>
		/// <remarks>Does not raise the GroupViewClosingEventArgs event.</remarks>
		public void CloseGroupView()
		{
			ClosePopup( null, false );
		}
		#endregion

		// Obsolete:

		#region AnimateTo(...)        
		/// <summary>
		///     Animate the scrolling of the list to the specified item.
		/// </summary>
		/// <param name="item">Item to scroll to.</param>
		[Obsolete( "AnimateTo(...) call ScrollTo(...) to jump without animation to the given item." )]
		public void AnimateTo( object item )
		{
			ScrollTo( item );
		}
		#endregion

		/// <summary>
		///     Returns either containers or items for either all items with
		///     containers or just the visible ones, as specified by the
		///     parameters.
		/// </summary>
		/// <param name="onlyItemsInView">
		///     When true, will return values for
		///     only items that are in view.
		/// </param>
		/// <param name="getContainers">
		///     When true, will return the containers
		///     rather than the items.
		/// </param>
		/// <returns>
		///     Returns either containers or items for either all items
		///     with containers or just the visible ones, as specified by the
		///     parameters.
		/// </returns>
		[SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "getContainers", Justification = "This is an obsolete old method that cannot change now." ), SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "onlyItemsInView", Justification = "This is an obsolete old method that cannot change now." ), SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is an obsolete old method that cannot change now." ), Obsolete( "GetItemsWithContainers(...) always returns an empty collection of items. Rely on Link/Unlink to know an item get realized or unrealized." )]
		public ICollection<Object> GetItemsWithContainers( bool onlyItemsInView, bool getContainers )
		{
			return new Collection<Object>();
		}

		#region GetItemsInView()
		/// <summary>
		///     Returns all of the items that are currently in view.
		///     This is not the same as the items that have associated visual elements: there are usually some visuals offscreen.
		///     This might be an empty list if scrolling is happening too quickly.
		/// </summary>
		/// <returns>Returns all of the items that are currently in view.</returns>
		[Obsolete( "GetItemsInView() always returns an empty collection of items. Rely on Link/Unlink to know an item get realized or unrealized." )]
		public ICollection<Object> GetItemsInView()
		{
			return GetItemsWithContainers( true, false );
		}
		#endregion

		#region OnItemsSourceChanged()
		/// <summary>
		///     Called when the ItemsSource dependency property changes.
		/// </summary>
		void OnItemsSourceChanged()
		{
			// Reload the whole list.
			LoadDataIntoListBox();
		}
		#endregion

		#region OnApplyTemplate()
		/// <summary>
		///     Called whenever a template gets applied on this control.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			// Unsubscribe from events we registered for in the past.
			if ( _listBox != null )
			{
				_listBox.SelectionChanged -= OnListBoxSelectionChanged;
				_listBox.Link -= OnLink;
				_listBox.Unlink -= OnUnlink;
			}

			if ( _scrollGroup != null )
			{
				_scrollGroup.CurrentStateChanging -= OnScrollStateChanging;
			}

			// Locates and setup the TemplatedListBox in the template. If no TemplatedListBox is found, we
			// initialize one.
			_listBox = GetTemplateChild( TemplatedListBoxName ) as TemplatedListBox ?? new TemplatedListBox();
			_listBox.ListHeaderTemplate = ListHeaderTemplate;
			_listBox.ListFooterTemplate = ListFooterTemplate;
			_listBox.GroupHeaderTemplate = GroupHeaderTemplate;
			_listBox.GroupFooterTemplate = GroupFooterTemplate;
			_listBox.ItemTemplate = ItemTemplate;
			_listBox.SelectionChanged += OnListBoxSelectionChanged;
			_listBox.Link += OnLink;
			_listBox.Unlink += OnUnlink;

			// Retrieves the ScrollViewer of the list box.
			ScrollViewer sv = _listBox.GetFirstLogicalChildByType<ScrollViewer>( true );

			if ( sv != null )
			{
				// Visual States are always on the first child of the control template 
				var element = VisualTreeHelper.GetChild( sv, 0 ) as FrameworkElement;
				if ( element != null )
				{
					_scrollGroup = VisualStateManager.GetVisualStateGroups( sv ).Cast<VisualStateGroup>().FirstOrDefault( x => x.Name == ScrollStatesGroupName );
					if ( _scrollGroup != null )
					{
						_scrollGroup.CurrentStateChanging += OnScrollStateChanging;
					}

					_verticalCompressionGroup = VisualStateManager.GetVisualStateGroups( sv ).Cast<VisualStateGroup>().FirstOrDefault( x => x.Name == VerticalCompressionStateName );
					if ( _verticalCompressionGroup != null )
					{
						_verticalCompressionGroup.CurrentStateChanging += OnStretchStateChanging;
					}
				}
			}

			LoadDataIntoListBox();
		}
		#endregion

		#region LoadDataIntoListBox()
		/// <summary>
		///     Loads ItemsSource into the hosted list box.
		/// </summary>
		void LoadDataIntoListBox()
		{
			if ( _listBox != null )
			{
				var tuples = new ObservableCollection<LongListSelectorItem>();

				AddListHeader( tuples );

				UnsubscribeFromAllCollections();

				// if it's a flat list, add the items without grouping.
				if ( IsFlatList )
				{
					if ( ItemsSource != null )
					{
						foreach ( var item in ItemsSource )
						{
							tuples.Add( new LongListSelectorItem { Item = item, ItemType = LongListSelectorItemType.Item } );
						}
					}
				}
				// Otherwise, apply the grouping logic.
				else
				{
					var groups = ItemsSource;
					if ( groups != null )
					{
						foreach ( var group in groups )
						{
							AddGroup( group, tuples );
						}
					}
				}

				AddListFooter( tuples );

				_rootCollection = ItemsSource as INotifyCollectionChanged;
				if ( _rootCollection != null )
				{
					_rootCollection.CollectionChanged += OnCollectionChanged;
				}

				_listBox.ItemsSource = tuples;
			}
		}
		#endregion

		#region  List/Footer Headers
		/// <summary>
		///     Adds a list header to the given list.
		/// </summary>
		void AddListHeader( IList<LongListSelectorItem> tuples )
		{
			if ( HasListHeader && ShowListHeader && // Adds the list header if it got a template or if it's a UI element itself.
				 ( tuples.Count == 0 || tuples[0].ItemType != LongListSelectorItemType.ListHeader ) ) // Also, make sure its not already there
			{
				tuples.Insert( 0, new LongListSelectorItem { Item = ListHeader, ItemType = LongListSelectorItemType.ListHeader } );
			}
		}

		/// <summary>
		///     Adds a list header to the listbox.
		/// </summary>
		void AddListHeader()
		{
			AddListHeader( (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource );
		}

		/// <summary>
		///     Removes the list header from the given list.
		/// </summary>
		static void RemoveListHeader( IList<LongListSelectorItem> tuples )
		{
			if ( tuples.Count > 0 && tuples[0].ItemType == LongListSelectorItemType.ListHeader )
			{
				tuples.RemoveAt( 0 );
			}
		}

		/// <summary>
		///     Removes the list header from the listbox.
		/// </summary>
		void RemoveListHeader()
		{
			RemoveListHeader( (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource );
		}

		/// <summary>
		///     Adds a list footer to the given list.
		/// </summary>
		void AddListFooter( IList<LongListSelectorItem> tuples )
		{
			if ( HasListFooter && ShowListFooter && // Adds the list footer if it got a template or if it's a UI element itself.
				 ( tuples.Count == 0 || tuples[tuples.Count - 1].ItemType != LongListSelectorItemType.ListFooter ) ) // Also, make sure its not already there
			{
				tuples.Add( new LongListSelectorItem { Item = ListFooter, ItemType = LongListSelectorItemType.ListFooter } );
			}
		}

		/// <summary>
		///     Adds a list footer to the listbox.
		/// </summary>
		void AddListFooter()
		{
			AddListFooter( (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource );
		}

		/// <summary>
		///     Removes the list footer from the given list.
		/// </summary>
		static void RemoveListFooter( IList<LongListSelectorItem> tuples )
		{
			var lastTuple = tuples[tuples.Count - 1];
			if ( lastTuple != null && lastTuple.ItemType == LongListSelectorItemType.ListFooter )
			{
				tuples.RemoveAt( tuples.Count - 1 );
			}
		}

		/// <summary>
		///     Removes the list footer from the listbox.
		/// </summary>
		void RemoveListFooter()
		{
			RemoveListFooter( (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource );
		}
		#endregion

		#region AddGroup(...)
		/// <summary>
		///     Adds a group to a list of tuples.
		/// </summary>
		/// <param name="groupToAdd">Group to add.</param>
		/// <param name="tuples">List to which the method will add the group.</param>
		void AddGroup( object groupToAdd, IList tuples )
		{
			var group = groupToAdd as IEnumerable;
			if ( group != null )
			{
				var groupHasItems = false;

				// Adds the group header
				if ( GroupHeaderTemplate != null )
				{
					tuples.Add( new LongListSelectorItem { Item = group, ItemType = LongListSelectorItemType.GroupHeader } );
				}

				// For each group header, add its items
				foreach ( var item in group )
				{
					tuples.Add( new LongListSelectorItem { Item = item, ItemType = LongListSelectorItemType.Item, Group = group } );
					groupHasItems = true;
				}

				// Add the group footer if the group has items or if we must display all groups whether or not they have items.
				if ( groupHasItems || DisplayAllGroups )
				{
					if ( GroupFooterTemplate != null )
					{
						tuples.Add( new LongListSelectorItem { Item = group, ItemType = LongListSelectorItemType.GroupFooter } );
					}
				}
				// Otherwise, remove the group header
				else if ( GroupHeaderTemplate != null )
				{
					tuples.RemoveAt( tuples.Count - 1 );
				}

				// Subscribe to collection change event
				var groupCollection = group as INotifyCollectionChanged;
				if ( groupCollection != null && !_groupCollections.Contains( groupCollection ) )
				{
					groupCollection.CollectionChanged += OnCollectionChanged;
					_groupCollections.Add( groupCollection );
				}
			}
		}
		#endregion

		#region AddGroupHeadersAndFooters(...)
		/// <summary>
		///     Adds group headers or footers after their template switch from being null
		///     to an actual value.
		/// </summary>
		/// <param name="addHeaders">Specifies whether or not to add group headers.</param>
		/// <param name="addFooters">Specifies whether or not to add group footers.</param>
		/// <remarks>
		///     Used only when templates for group headers/footers switch from being null.
		///     In this case, they must be added to the lisbox if a group is not empty or DisplayAllGroups is true.
		///     For performance reasons, this method makes an assumption that headers/footers are not already present.
		///     Which is the case when its called from OnDataTemplateChanged.
		/// </remarks>
		void AddGroupHeadersAndFooters( bool addHeaders, bool addFooters )
		{
			var indexInListBox = 0;

			if ( HasListHeader && ShowListHeader )
			{
				++indexInListBox;
			}

			var groups = ItemsSource;
			var tuples = (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource;

			if ( groups != null )
			{
				foreach ( var group in groups )
				{
					var groupAsEnumerable = group as IEnumerable;
					if ( groupAsEnumerable != null )
					{
						var itemsCount = GetHeadersAndItemsCountFromGroup( groupAsEnumerable );

						// Adds the group header
						if ( addHeaders && GroupHeaderTemplate != null && itemsCount > 0 )
						{
							tuples.Insert( indexInListBox, new LongListSelectorItem
							{
								Item = group,
								ItemType = LongListSelectorItemType.GroupHeader
							} );
						}

						indexInListBox += itemsCount;

						// Adds the group footer
						if ( addFooters && GroupFooterTemplate != null && itemsCount > 0 )
						{
							tuples.Insert( indexInListBox - 1, new LongListSelectorItem
							{
								Item = group,
								ItemType = LongListSelectorItemType.GroupFooter
							} );
						}
					}
				}
			}
		}

		/// <summary>
		///     Adds group headers after the GroupHeaderTeamplate switch from being null
		///     to an actual value.
		/// </summary>
		void AddGroupHeaders()
		{
			AddGroupHeadersAndFooters( true, false );
		}

		/// <summary>
		///     Adds group headers after the GroupFooterTeamplate switch from being null
		///     to an actual value.
		/// </summary>
		void AddGroupFooters()
		{
			AddGroupHeadersAndFooters( false, true );
		}
		#endregion

		#region RemoveAllGroupHeadersAndFooters(...)
		/// <summary>
		///     Removes group headers or footers after their template becomes null.
		/// </summary>
		/// <param name="removeHeaders">Specifies whether or not to remove group headers.</param>
		/// <param name="removeFooters">Specifies whether or not to remove group footers.</param>
		void RemoveAllGroupHeadersAndFooters( bool removeHeaders, bool removeFooters )
		{
			var tuples = (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource;
			for ( var i = 0; i < tuples.Count; ++i )
			{
				if ( ( removeHeaders && tuples[i].ItemType == LongListSelectorItemType.GroupHeader ) ||
					 ( removeFooters && tuples[i].ItemType == LongListSelectorItemType.GroupFooter ) )
				{
					tuples.RemoveAt( i-- ); // the -- is there so we don't skip tuples
				}
			}
		}

		void RemoveAllGroupHeaders()
		{
			RemoveAllGroupHeadersAndFooters( true, false );
		}

		void RemoveAllGroupFooters()
		{
			RemoveAllGroupHeadersAndFooters( false, true );
		}
		#endregion

		#region UnsubscribeFromAllCollections()
		/// <summary>
		///     Unsubscrives from every collection in ItemsSource.
		/// </summary>
		void UnsubscribeFromAllCollections()
		{
			if ( _rootCollection != null )
			{
				_rootCollection.CollectionChanged -= OnCollectionChanged;
			}

			foreach ( var collection in _groupCollections )
			{
				collection.CollectionChanged -= OnCollectionChanged;
			}

			_rootCollection = null;
			_groupCollections.Clear();
		}
		#endregion

		#region InsertNewGroups(...)
		/// <summary>
		///     Inserts new groups in the list box.
		/// </summary>
		/// <param name="newGroups">List of the new groups to insert.</param>
		/// <param name="newGroupsIndex">Insertion index relative to the collection.</param>
		void InsertNewGroups( IList newGroups, int newGroupsIndex )
		{
			var tuples = (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource;

			// 1 - Builds items tuples for the new groups
			var newGroupsTuples = new List<LongListSelectorItem>();

			foreach ( var group in newGroups )
			{
				AddGroup( group, newGroupsTuples );
			}

			if ( newGroupsTuples.Count > 0 )
			{
				// 2 - Finds insertion index in the list box
				var insertIndex = GetGroupIndexInListBox( newGroupsIndex );

				// 3 - Inserts the new items into the list box
				foreach ( var tuple in newGroupsTuples )
				{
					tuples.Insert( insertIndex++, tuple );
				}
			}
		}
		#endregion

		#region InsertNewItems(...)
		/// <summary>
		///     Inserts new items in the list box.
		/// </summary>
		/// <param name="newItems">List of new items to insert.</param>
		/// <param name="newItemsIndex">Insertion index relative to the collection</param>
		/// <param name="group">Group into which the items are inserted. Can be null if IsFlatList == true</param>
		void InsertNewItems( IList newItems, int newItemsIndex, IEnumerable group )
		{
			var tuples = (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource;

			// 1 - Builds items tuples for the new items
			var newItemsTuples = new List<LongListSelectorItem>();
			foreach ( var item in newItems )
			{
				newItemsTuples.Add( new LongListSelectorItem
				{
					Group = group,
					Item = item,
					ItemType = LongListSelectorItemType.Item
				} );
			}

			// 2 - Finds the insertion index in the listbox
			// Since a single group might be referenced by more than one, we might need to update more than one spot in the listbox
			if ( group != null )
			{
				var i = 0;
				var groupWasNotDisplayed = ( (IList)group ).Count == newItems.Count && !DisplayAllGroups;

				foreach ( var g in ItemsSource )
				{
					if ( g == group )
					{
						var insertIndex = GetGroupIndexInListBox( i );

						if ( GroupHeaderTemplate != null )
						{
							if ( groupWasNotDisplayed )
							{
								tuples.Insert( insertIndex, new LongListSelectorItem { ItemType = LongListSelectorItemType.GroupHeader, Item = group } );
							}
							++insertIndex;
						}

						insertIndex += newItemsIndex;

						// 3 - Inserts the new items into the list box
						foreach ( var tuple in newItemsTuples )
						{
							tuples.Insert( insertIndex++, tuple );
						}

						if ( groupWasNotDisplayed && GroupFooterTemplate != null )
						{
							tuples.Insert( insertIndex++, new LongListSelectorItem { ItemType = LongListSelectorItemType.GroupFooter, Item = group } );
						}
					}
					++i;
				}
			}
			else
			{
				var insertIndex = newItemsIndex;
				if ( HasListHeader && ShowListHeader )
				{
					++insertIndex;
				}

				// 3 - Inserts the new items into the list box
				foreach ( var tuple in newItemsTuples )
				{
					tuples.Insert( insertIndex++, tuple );
				}
			}
		}
		#endregion

		#region RemoveOldGroups(...)
		/// <summary>
		///     Removes groups from the list box.
		/// </summary>
		/// <param name="oldGroups">List of groups to remove.</param>
		/// <param name="oldGroupsIndex">Start index relative to the root collection.</param>
		void RemoveOldGroups( IList oldGroups, int oldGroupsIndex )
		{
			var tuples = (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource;

			// 1 - Find the index at which we start removing groups
			var removeStartIndex = 0;
			if ( oldGroupsIndex > 0 )
			{
				removeStartIndex = GetGroupIndexInListBox( oldGroupsIndex - 1 );
				var group = ( (IList)ItemsSource )[oldGroupsIndex - 1] as IEnumerable;
				if ( group != null )
				{
					removeStartIndex += GetHeadersAndItemsCountFromGroup( group );
				}
			}
			else
			{
				if ( HasListHeader && ShowListHeader )
				{
					++removeStartIndex;
				}
			}

			// 2 - Calculates how many items to delete from the ListBox
			var itemsToRemoveCount = GetItemsCountFromGroups( oldGroups );

			// 3 - Removes the old items from the ListBox
			for ( var i = 0; i < itemsToRemoveCount; ++i )
			{
				tuples.RemoveAt( removeStartIndex );
			}

			// 4 - Unsubscribe from the old groups
			foreach ( INotifyCollectionChanged collection in oldGroups )
			{
				collection.CollectionChanged -= OnCollectionChanged;
			}
		}
		#endregion

		#region RemoveOldItems(...)
		/// <summary>
		///     Removes old items from a group or from the root collection.
		/// </summary>
		/// <param name="oldItems">List of items to remove.</param>
		/// <param name="oldItemsIndex">Start index relative to the group or root collection.</param>
		/// <param name="group">Group from which items are removed. Can be null if IsFlatList == true.</param>
		void RemoveOldItems( IList oldItems, int oldItemsIndex, IEnumerable group )
		{
			var tuples = (ObservableCollection<LongListSelectorItem>)_listBox.ItemsSource;

			// 1 - Finds the remove index in the listbox
			// Since a single group might be referenced by more than one, we might need to update more than one group
			if ( group != null )
			{
				var i = 0;

				foreach ( var g in ItemsSource )
				{
					if ( g == group )
					{
						var removeIndex = GetGroupIndexInListBox( i );
						removeIndex += oldItemsIndex;

						if ( GroupHeaderTemplate != null )
						{
							++removeIndex;
						}

						// 2 - Removes the old items
						for ( var j = 0; j < oldItems.Count; ++j )
						{
							tuples.RemoveAt( removeIndex );
						}

						// 3 - Hides the group header and footer if it's empty and DisplayAllGroups is false
						if ( ( (IList)group ).Count == 0 && !DisplayAllGroups )
						{
							if ( GroupFooterTemplate != null )
							{
								tuples.RemoveAt( removeIndex ); // Removes the group footer
							}
							if ( GroupHeaderTemplate != null )
							{
								tuples.RemoveAt( removeIndex - 1 ); // Removes the group header
							}
						}
					}
					++i;
				}
			}
			else
			{
				var removeIndex = oldItemsIndex;
				if ( HasListHeader && ShowListHeader )
				{
					++removeIndex;
				}
				for ( var i = 0; i < oldItems.Count; ++i )
				{
					tuples.RemoveAt( removeIndex );
				}
			}
		}
		#endregion

		#region GetGroupIndexInListBox(...)
		/// <summary>
		///     Returns, for a group, an index relative to the templated list box from an index relative to the root collection.
		/// </summary>
		/// <param name="indexInLLS">Index relative to the root collection.</param>
		/// <returns>
		///     Returns, for a group, an index relative to the templated list box from an index relative to the root
		///     collection.
		/// </returns>
		int GetGroupIndexInListBox( int indexInLLS )
		{
			int indexInListBox = 0, index = 0;

			if ( HasListHeader && ShowListHeader )
			{
				++indexInListBox;
			}

			var groups = ItemsSource;

			if ( groups != null )
			{
				foreach ( var group in groups )
				{
					if ( indexInLLS == index )
					{
						break;
					}

					++index;

					var groupAsEnumerable = group as IEnumerable;
					if ( groupAsEnumerable != null )
					{
						indexInListBox += GetHeadersAndItemsCountFromGroup( groupAsEnumerable );
					}
				}
			}

			return indexInListBox;
		}
		#endregion

		#region GetItemsCountFromGroups(...)
		/// <summary>
		///     Returns the number of items in a list of groups.
		/// </summary>
		/// <param name="groups">List of groups from which to retrieve the items count.</param>
		/// <returns>Returns the number of items in a list of groups.</returns>
		int GetItemsCountFromGroups( IEnumerable groups )
		{
			var count = 0;

			foreach ( var g in groups )
			{
				var group = g as IEnumerable;
				if ( group != null )
				{
					count += GetHeadersAndItemsCountFromGroup( group );
				}
			}

			return count;
		}
		#endregion

		#region GetItemsCountFromGroup(...)
		/// <summary>
		///     Returns the number of items in a group including the group header.
		/// </summary>
		/// <param name="group">Group from which to retrieve the items count.</param>
		/// <returns>Returns the number of items in a group including the group header.</returns>
		int GetHeadersAndItemsCountFromGroup( IEnumerable group )
		{
			var count = 0;

			var groupAsList = group as IList;
			if ( groupAsList != null )
			{
				count += groupAsList.Count;
			}
			else
			{
				count += group.Cast<object>().Count();
			}

			var groupHasItems = count > 0;

			if ( ( groupHasItems || DisplayAllGroups ) && GroupHeaderTemplate != null )
			{
				++count;
			}

			if ( ( groupHasItems || DisplayAllGroups ) && GroupFooterTemplate != null )
			{
				++count;
			}

			return count;
		}
		#endregion

		#region UpdateListBoxItemsTemplate(...)
		/// <summary>
		///     Updates the templates for a given item type in the list box.
		/// </summary>
		/// <param name="itemType">Item type for which to update the template.</param>
		/// <param name="newTemplate">New template that will replace the old one.</param>
		void UpdateItemsTemplate( LongListSelectorItemType itemType, DataTemplate newTemplate )
		{
			if ( _listBox != null )
			{
				// Update template for items that have been linked (realized)
				IEnumerable<TemplatedListBoxItem> items = _listBox.GetLogicalChildrenByType<TemplatedListBoxItem>( false );
				foreach ( var item in items )
				{
					var tuple = item.Tuple;
					if ( tuple.ItemType == itemType )
					{
						item.ContentTemplate = newTemplate;
					}
				}

				// Save the new template so they can be applied in the future when new items
				// are linked (realized)
				switch ( itemType )
				{
					case LongListSelectorItemType.ListHeader:
						_listBox.ListHeaderTemplate = newTemplate;
						break;
					case LongListSelectorItemType.ListFooter:
						_listBox.ListFooterTemplate = newTemplate;
						break;
					case LongListSelectorItemType.GroupHeader:
						_listBox.GroupHeaderTemplate = newTemplate;
						break;
					case LongListSelectorItemType.GroupFooter:
						_listBox.GroupFooterTemplate = newTemplate;
						break;
					case LongListSelectorItemType.Item:
						_listBox.ItemTemplate = newTemplate;
						break;
				}
			}
		}
		#endregion

		#region OnDataTemplateChanged(...)
		/// <summary>
		///     Called when a data template has changed.
		/// </summary>
		static void OnDataTemplateChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			var lls = (LongListSelector)o;
			if ( lls._listBox == null )
			{
				return;
			}

			var newTemplate = (DataTemplate)e.NewValue;

			if ( e.Property == ListHeaderTemplateProperty )
			{
				lls.UpdateItemsTemplate( LongListSelectorItemType.ListHeader, newTemplate );

				// If the old value was null, we might need to add the list header.
				if ( e.OldValue == null )
				{
					lls.AddListHeader(); // Will add a list header if it's not already there.
				}
				// If we don't have a list header anymore, then remove it from the listbox
				else if ( e.NewValue == null && !lls.HasListHeader )
				{
					lls.RemoveListHeader();
				}
			}
			else if ( e.Property == ListFooterTemplateProperty )
			{
				lls.UpdateItemsTemplate( LongListSelectorItemType.ListFooter, newTemplate );

				// If the old value was null, we might need to add the list footer.
				if ( e.OldValue == null )
				{
					lls.AddListFooter(); // Will add a list footer if it's not already there.
				}
				// If we don't have a list footer anymore, then remove it from the listbox
				else if ( e.NewValue == null && !lls.HasListHeader )
				{
					lls.RemoveListFooter();
				}
			}
			else if ( e.Property == GroupHeaderProperty )
			{
				lls.UpdateItemsTemplate( LongListSelectorItemType.GroupHeader, newTemplate );

				// If the old value was null, this means we might need to add group headers to the listbox
				if ( e.OldValue == null )
				{
					lls.AddGroupHeaders();
				}
				// If the new value is null, this means we might need to remove group headers from the listbox
				else if ( e.NewValue == null )
				{
					lls.RemoveAllGroupHeaders();
				}
			}
			else if ( e.Property == GroupFooterProperty )
			{
				lls.UpdateItemsTemplate( LongListSelectorItemType.GroupFooter, newTemplate );

				// If the old value was null, this means we might need to add group footers to the listbox
				if ( e.OldValue == null )
				{
					lls.AddGroupFooters();
				}
				// If the new value is null, this means we might need to remove group footers from the listbox
				else if ( e.NewValue == null )
				{
					lls.RemoveAllGroupFooters();
				}
			}
			else if ( e.Property == ItemsTemplateProperty )
			{
				lls.UpdateItemsTemplate( LongListSelectorItemType.Item, newTemplate );
			}
		}
		#endregion

		#region OnDisplayAllGroupsChanged(...)
		/// <summary>
		///     Called when the DisplayAllGroups dependency property changes
		/// </summary>
		static void OnDisplayAllGroupsChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			( (LongListSelector)obj ).LoadDataIntoListBox();
		}
		#endregion

		#region OnListBoxSelectionChanged(...)
		/// <summary>
		///     Called when there is a change in the selected item(s) in the listbox.
		/// </summary>
		void OnListBoxSelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			// Group navigation
			//var group = (from t in (IEnumerable<object>)e.AddedItems where ((ItemTuple)t).ItemType == ItemType.GroupHeader select (ItemTuple)t).FirstOrDefault();
			LongListSelectorItem group = null;
			foreach ( LongListSelectorItem tuple in e.AddedItems )
			{
				if ( tuple.ItemType == LongListSelectorItemType.GroupHeader )
				{
					group = tuple;
					break;
				}
			}

			if ( group != null )
			{
				if ( _listBox != null )
				{
					_listBox.SelectedItem = null;
				}
				SelectedItem = null;
				DisplayGroupView();
			}
			else
			{
				if ( e.AddedItems.Count > 0 && ( (LongListSelectorItem)e.AddedItems[0] ).ItemType == LongListSelectorItemType.Item )
				{
					SelectedItem = ( (LongListSelectorItem)e.AddedItems[0] ).Item;
				}
				else
				{
					if ( _listBox != null )
					{
						_listBox.SelectedItem = null;
					}
					SelectedItem = null;
				}
			}
		}
		#endregion

		#region OnCollectionChanged(...)
		/// <summary>
		///     Called when there is a change in the root or a group collection.
		/// </summary>
		void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			var senderAsEnumerable = sender as IEnumerable;

			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
					if ( sender == _rootCollection )
					{
						if ( IsFlatList )
						{
							InsertNewItems( e.NewItems, e.NewStartingIndex, null );
						}
						else
						{
							InsertNewGroups( e.NewItems, e.NewStartingIndex );
						}
					}
					else
					{
						InsertNewItems( e.NewItems, e.NewStartingIndex, senderAsEnumerable );
					}
					break;

				case NotifyCollectionChangedAction.Remove:
					if ( sender == _rootCollection )
					{
						if ( IsFlatList )
						{
							RemoveOldItems( e.OldItems, e.OldStartingIndex, null );
						}
						else
						{
							RemoveOldGroups( e.OldItems, e.OldStartingIndex );
						}
					}
					else
					{
						RemoveOldItems( e.OldItems, e.OldStartingIndex, senderAsEnumerable );
					}
					break;

				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Reset:
					LoadDataIntoListBox();
					break;
			}
		}
		#endregion

		#region OnScrollStateChanging(...)
		/// <summary>
		///     Called when the scrolling state of the list box changes.
		/// </summary>
		void OnScrollStateChanging( object sender, VisualStateChangedEventArgs e )
		{
			IsScrolling = e.NewState.Name == ScrollingState;

			if ( e.NewState.Name == ScrollingState && ScrollingStarted != null )
			{
				ScrollingStarted( this, null );
			}
			else if ( e.NewState.Name == NotScrollingState && ScrollingCompleted != null )
			{
				ScrollingCompleted( this, null );
			}
		}
		#endregion

		#region OnScrollStateChanging(...)
		/// <summary>
		///     Called when the scrolling state of the list box changes.
		/// </summary>
		void OnStretchStateChanging( object sender, VisualStateChangedEventArgs e )
		{
			IsStretching = e.NewState.Name == CompressionBottom || e.NewState.Name == CompressionTop;

			if ( e.NewState.Name == CompressionTop && StretchingTop != null )
			{
				StretchingTop( this, null );
			}
			else if ( e.NewState.Name == CompressionBottom && StretchingBottom != null )
			{
				StretchingBottom( this, null );
			}
			else if ( e.NewState.Name == NoVerticalCompression && StretchingCompleted != null )
			{
				StretchingCompleted( this, null );
			}
		}
		#endregion

		#region OnLink(...)
		/// <summary>
		///     Called when an item gets realized.
		/// </summary>
		void OnLink( object sender, LinkUnlinkEventArgs e )
		{
			if ( Link != null )
			{
				Link( this, e );
			}
		}
		#endregion

		#region OnUnlink(...)
		/// <summary>
		///     Called when an item gets unrealized.
		/// </summary>
		void OnUnlink( object sender, LinkUnlinkEventArgs e )
		{
			if ( Unlink != null )
			{
				Unlink( this, e );
			}
		}
		#endregion
	}
}