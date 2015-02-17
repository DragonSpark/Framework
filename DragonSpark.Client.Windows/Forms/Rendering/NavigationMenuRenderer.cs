using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Xamarin.Forms;
using ColumnDefinition = System.Windows.Controls.ColumnDefinition;
using Grid = System.Windows.Controls.Grid;
using GridLength = System.Windows.GridLength;
using ImageSource = System.Windows.Media.ImageSource;
using IValueConverter = System.Windows.Data.IValueConverter;
using RowDefinition = System.Windows.Controls.RowDefinition;
using Thickness = System.Windows.Thickness;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public enum TileSize
	{
		Default,
		Small,
		Medium,
		Large
	}

	enum ImageState
	{
		Expanded,
		Semiexpanded,
		Collapsed,
		Flipped
	}

	public static class HubTileService
{
    // Fields
    private static List<WeakReference> EnabledImagesPool = new List<WeakReference>();
    private static List<WeakReference> FrozenImagesPool = new List<WeakReference>();
    private const int NumberOfSimultaneousAnimations = 1;
    private static Random ProbabilisticBehaviorSelector = new Random();
    private static List<WeakReference> StalledImagesPipeline = new List<WeakReference>();
    private static DispatcherTimer Timer = new DispatcherTimer();
    private const bool TrackResurrection = false;
    private const int WaitingPipelineSteps = 3;

    // Methods
    static HubTileService()
    {
        Timer.Tick += new EventHandler(HubTileService.OnTimerTick);
    }

    private static void AddReferenceToEnabledPool(WeakReference tile)
    {
        if (!ContainsTarget(EnabledImagesPool, tile.Target))
        {
            EnabledImagesPool.Add(tile);
        }
    }

    private static void AddReferenceToFrozenPool(WeakReference tile)
    {
        if (!ContainsTarget(FrozenImagesPool, tile.Target))
        {
            FrozenImagesPool.Add(tile);
        }
    }

    private static void AddReferenceToStalledPipeline(WeakReference tile)
    {
        if (!ContainsTarget(StalledImagesPipeline, tile.Target))
        {
            StalledImagesPipeline.Add(tile);
        }
    }

    private static bool ContainsTarget(List<WeakReference> list, object target)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Target == target)
            {
                return true;
            }
        }
        return false;
    }

    internal static void FinalizeReference(HubTile tile)
    {
        WeakReference reference = new WeakReference(tile, false);
        RemoveReferenceFromEnabledPool(reference);
        RemoveReferenceFromFrozenPool(reference);
        RemoveReferenceFromStalledPipeline(reference);
    }

    public static void FreezeGroup(string group)
    {
        for (int i = 0; i < EnabledImagesPool.Count; i++)
        {
            if ((EnabledImagesPool[i].Target as HubTile).GroupTag == group)
            {
                (EnabledImagesPool[i].Target as HubTile).IsFrozen = true;
                i--;
            }
        }
        for (int j = 0; j < StalledImagesPipeline.Count; j++)
        {
            if ((StalledImagesPipeline[j].Target as HubTile).GroupTag == group)
            {
                (StalledImagesPipeline[j].Target as HubTile).IsFrozen = true;
                j--;
            }
        }
    }

    public static void FreezeHubTile(HubTile tile)
    {
        WeakReference reference = new WeakReference(tile, false);
        AddReferenceToFrozenPool(reference);
        RemoveReferenceFromEnabledPool(reference);
        RemoveReferenceFromStalledPipeline(reference);
    }

    internal static void InitializeReference(HubTile tile)
    {
        WeakReference reference = new WeakReference(tile, false);
        if (tile.IsFrozen)
        {
            AddReferenceToFrozenPool(reference);
        }
        else
        {
            AddReferenceToEnabledPool(reference);
        }
        RestartTimer();
    }

    private static void OnTimerTick(object sender, EventArgs e)
    {
        Timer.Stop();
        for (int i = 0; i < StalledImagesPipeline.Count; i++)
        {
            HubTile target = StalledImagesPipeline[i].Target as HubTile;
            if (target._stallingCounter-- == 0)
            {
                AddReferenceToEnabledPool(StalledImagesPipeline[i]);
                RemoveReferenceFromStalledPipeline(StalledImagesPipeline[i]);
                i--;
            }
        }
        if (EnabledImagesPool.Count > 0)
        {
            for (int j = 0; j < 1; j++)
            {
                int num3 = ProbabilisticBehaviorSelector.Next(EnabledImagesPool.Count);
                switch ((EnabledImagesPool[num3].Target as HubTile).State)
                {
                    case ImageState.Expanded:
                        if (((EnabledImagesPool[num3].Target as HubTile)._canDrop || (EnabledImagesPool[num3].Target as HubTile)._canFlip) && ((EnabledImagesPool[num3].Target as HubTile).Size != TileSize.Small))
                        {
                            if (!(EnabledImagesPool[num3].Target as HubTile)._canDrop && (EnabledImagesPool[num3].Target as HubTile)._canFlip)
                            {
                                (EnabledImagesPool[num3].Target as HubTile).State = ImageState.Flipped;
                            }
                            else if (!(EnabledImagesPool[num3].Target as HubTile)._canFlip && (EnabledImagesPool[num3].Target as HubTile)._canDrop)
                            {
                                (EnabledImagesPool[num3].Target as HubTile).State = ImageState.Semiexpanded;
                            }
                            else if (ProbabilisticBehaviorSelector.Next(2) == 0)
                            {
                                (EnabledImagesPool[num3].Target as HubTile).State = ImageState.Semiexpanded;
                            }
                            else
                            {
                                (EnabledImagesPool[num3].Target as HubTile).State = ImageState.Flipped;
                            }
                        }
                        break;

                    case ImageState.Semiexpanded:
                        (EnabledImagesPool[num3].Target as HubTile).State = ImageState.Collapsed;
                        break;

                    case ImageState.Collapsed:
                        (EnabledImagesPool[num3].Target as HubTile).State = ImageState.Expanded;
                        break;

                    case ImageState.Flipped:
                        (EnabledImagesPool[num3].Target as HubTile).State = ImageState.Expanded;
                        break;
                }
                (EnabledImagesPool[num3].Target as HubTile)._stallingCounter = 3;
                AddReferenceToStalledPipeline(EnabledImagesPool[num3]);
                RemoveReferenceFromEnabledPool(EnabledImagesPool[num3]);
            }
        }
        else if (StalledImagesPipeline.Count == 0)
        {
            return;
        }
        Timer.Interval = TimeSpan.FromMilliseconds((double) (ProbabilisticBehaviorSelector.Next(1, 0x1f) * 100));
        Timer.Start();
    }

    private static void RemoveReferenceFromEnabledPool(WeakReference tile)
    {
        RemoveTarget(EnabledImagesPool, tile.Target);
    }

    private static void RemoveReferenceFromFrozenPool(WeakReference tile)
    {
        RemoveTarget(FrozenImagesPool, tile.Target);
    }

    private static void RemoveReferenceFromStalledPipeline(WeakReference tile)
    {
        RemoveTarget(StalledImagesPipeline, tile.Target);
    }

    private static void RemoveTarget(List<WeakReference> list, object target)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Target == target)
            {
                list.RemoveAt(i);
                return;
            }
        }
    }

    private static void RestartTimer()
    {
        if (!Timer.IsEnabled)
        {
            Timer.Interval = TimeSpan.FromMilliseconds(2500.0);
            Timer.Start();
        }
    }

    public static void UnfreezeGroup(string group)
    {
        for (int i = 0; i < FrozenImagesPool.Count; i++)
        {
            if ((FrozenImagesPool[i].Target as HubTile).GroupTag == group)
            {
                (FrozenImagesPool[i].Target as HubTile).IsFrozen = false;
                i--;
            }
        }
        RestartTimer();
    }

    public static void UnfreezeHubTile(HubTile tile)
    {
        WeakReference reference = new WeakReference(tile, false);
        AddReferenceToEnabledPool(reference);
        RemoveReferenceFromFrozenPool(reference);
        RemoveReferenceFromStalledPipeline(reference);
        RestartTimer();
    }
}



	[TemplateVisualState( Name = "Expanded", GroupName = "ImageState" ), TemplatePart( Name = "BackTitleBlock", Type = typeof(TextBlock) ), TemplatePart( Name = "MessageBlock", Type = typeof(TextBlock) ), TemplateVisualState( Name = "Flipped", GroupName = "ImageState" ), TemplatePart( Name = "NotificationBlock", Type = typeof(TextBlock) ), TemplatePart( Name = "TitlePanel", Type = typeof(Panel) ), TemplateVisualState( Name = "Collapsed", GroupName = "ImageState" ), TemplateVisualState( Name = "Semiexpanded", GroupName = "ImageState" )]
	public class HubTile : Control
	{
		// Fields
		TextBlock _backTitleBlock;
		internal bool _canDrop;
		internal bool _canFlip;
		TextBlock _messageBlock;
		TextBlock _notificationBlock;
		internal int _stallingCounter;
		Panel _titlePanel;
		const string BackTitleBlock = "BackTitleBlock";
		const string Collapsed = "Collapsed";
		public static readonly DependencyProperty DisplayNotificationProperty = DependencyProperty.Register( "DisplayNotification", typeof(bool), typeof(HubTile), new PropertyMetadata( false, OnBackContentChanged ) );
		const string Expanded = "Expanded";
		const string Flipped = "Flipped";
		public static readonly DependencyProperty GroupTagProperty = DependencyProperty.Register( "GroupTag", typeof(string), typeof(HubTile), new PropertyMetadata( string.Empty ) );
		const string ImageStates = "ImageState";
		public static readonly DependencyProperty IsFrozenProperty = DependencyProperty.Register( "IsFrozen", typeof(bool), typeof(HubTile), new PropertyMetadata( false, OnIsFrozenChanged ) );
		const string MessageBlock = "MessageBlock";
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register( "Message", typeof(string), typeof(HubTile), new PropertyMetadata( string.Empty, OnBackContentChanged ) );
		const string NotificationBlock = "NotificationBlock";
		public static readonly DependencyProperty NotificationProperty = DependencyProperty.Register( "Notification", typeof(string), typeof(HubTile), new PropertyMetadata( string.Empty, OnBackContentChanged ) );
		const string Semiexpanded = "Semiexpanded";
		public static readonly DependencyProperty SizeProperty = DependencyProperty.Register( "Size", typeof(TileSize), typeof(HubTile), new PropertyMetadata( TileSize.Default, OnSizeChanged ) );
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register( "Source", typeof(ImageSource), typeof(HubTile), new PropertyMetadata( null ) );
		static readonly DependencyProperty StateProperty = DependencyProperty.Register( "State", typeof(ImageState), typeof(HubTile), new PropertyMetadata( ImageState.Expanded, OnImageStateChanged ) );
		const string TitlePanel = "TitlePanel";
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register( "Title", typeof(string), typeof(HubTile), new PropertyMetadata( string.Empty, OnTitleChanged ) );

		// Methods
		public HubTile()
		{
			DefaultStyleKey = typeof(HubTile);
			Loaded += HubTile_Loaded;
			Unloaded += HubTile_Unloaded;
		}

		void HubTile_Loaded( object sender, RoutedEventArgs e )
		{
			HubTileService.InitializeReference( this );
		}

		void HubTile_Unloaded( object sender, RoutedEventArgs e )
		{
			HubTileService.FinalizeReference( this );
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_notificationBlock = GetTemplateChild( "NotificationBlock" ) as TextBlock;
			_messageBlock = GetTemplateChild( "MessageBlock" ) as TextBlock;
			_backTitleBlock = GetTemplateChild( "BackTitleBlock" ) as TextBlock;
			_titlePanel = GetTemplateChild( "TitlePanel" ) as Panel;
			if ( _notificationBlock != null )
			{
				var binding = new System.Windows.Data.Binding
				{
					Source = this,
					Path = new PropertyPath( "DisplayNotification" ),
					Converter = new VisibilityConverter(),
					ConverterParameter = false
				};
				_notificationBlock.SetBinding( VisibilityProperty, binding );
			}
			if ( _messageBlock != null )
			{
				var binding2 = new System.Windows.Data.Binding
				{
					Source = this,
					Path = new PropertyPath( "DisplayNotification" ),
					Converter = new VisibilityConverter(),
					ConverterParameter = true
				};
				_messageBlock.SetBinding( VisibilityProperty, binding2 );
			}
			if ( _backTitleBlock != null )
			{
				var binding3 = new System.Windows.Data.Binding
				{
					Source = this,
					Path = new PropertyPath( "Title" ),
					Converter = new MultipleToSingleLineStringConverter()
				};
				_backTitleBlock.SetBinding( TextBlock.TextProperty, binding3 );
			}
			UpdateVisualState();
		}

		static void OnBackContentChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			var tile = (HubTile)obj;
			if ( ( !string.IsNullOrEmpty( tile.Notification ) && tile.DisplayNotification ) || ( !string.IsNullOrEmpty( tile.Message ) && !tile.DisplayNotification ) )
			{
				tile._canFlip = true;
			}
			else
			{
				tile._canFlip = false;
				tile.State = ImageState.Expanded;
			}
		}

		static void OnHubTileSizeChanged( object sender, SizeChangedEventArgs e )
		{
			var control = (HubTile)sender;
			control.SizeChanged -= OnHubTileSizeChanged;
			if ( control.State != ImageState.Expanded )
			{
				control.State = ImageState.Expanded;
				VisualStateManager.GoToState( control, "Expanded", false );
			}
			else if ( control._titlePanel != null )
			{
				/*CompositeTransform renderTransform = control._titlePanel.RenderTransform as CompositeTransform;
				if ( renderTransform != null )
				{
					renderTransform.TranslateY = -control.Height;
				}*/
			}
			HubTileService.InitializeReference( control );
		}

		static void OnImageStateChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			( (HubTile)obj ).UpdateVisualState();
		}

		static void OnIsFrozenChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			var tile = (HubTile)obj;
			if ( (bool)e.NewValue )
			{
				HubTileService.FreezeHubTile( tile );
			}
			else
			{
				HubTileService.UnfreezeHubTile( tile );
			}
		}

		static void OnSizeChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			var tile = (HubTile)obj;
			switch ( tile.Size )
			{
				case TileSize.Default:
					tile.Width = 173.0;
					tile.Height = 173.0;
					break;

				case TileSize.Small:
					tile.Width = 99.0;
					tile.Height = 99.0;
					break;

				case TileSize.Medium:
					tile.Width = 210.0;
					tile.Height = 210.0;
					break;

				case TileSize.Large:
					tile.Width = 432.0;
					tile.Height = 210.0;
					break;
			}
			tile.SizeChanged += OnHubTileSizeChanged;
			HubTileService.FinalizeReference( tile );
		}

		static void OnTitleChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
		{
			var tile = (HubTile)obj;
			if ( string.IsNullOrEmpty( (string)e.NewValue ) )
			{
				tile._canDrop = false;
				tile.State = ImageState.Expanded;
			}
			else
			{
				tile._canDrop = true;
			}
		}

		void UpdateVisualState()
		{
			string str;
			if ( Size != TileSize.Small )
			{
				switch ( State )
				{
					case ImageState.Expanded:
						str = "Expanded";
						goto Label_0056;

					case ImageState.Semiexpanded:
						str = "Semiexpanded";
						goto Label_0056;

					case ImageState.Collapsed:
						str = "Collapsed";
						goto Label_0056;

					case ImageState.Flipped:
						str = "Flipped";
						goto Label_0056;
				}
				str = "Expanded";
			}
			else
			{
				str = "Expanded";
			}
			Label_0056:
			VisualStateManager.GoToState( this, str, true );
		}

		// Properties
		public bool DisplayNotification
		{
			get { return (bool)GetValue( DisplayNotificationProperty ); }
			set { SetValue( DisplayNotificationProperty, value ); }
		}

		public string GroupTag
		{
			get { return (string)GetValue( GroupTagProperty ); }
			set { SetValue( GroupTagProperty, value ); }
		}

		public bool IsFrozen
		{
			get { return (bool)GetValue( IsFrozenProperty ); }
			set { SetValue( IsFrozenProperty, value ); }
		}

		public string Message
		{
			get { return (string)GetValue( MessageProperty ); }
			set { SetValue( MessageProperty, value ); }
		}

		public string Notification
		{
			get { return (string)GetValue( NotificationProperty ); }
			set { SetValue( NotificationProperty, value ); }
		}

		public TileSize Size
		{
			get { return (TileSize)GetValue( SizeProperty ); }
			set { SetValue( SizeProperty, value ); }
		}

		public System.Windows.Media.ImageSource Source
		{
			get { return (System.Windows.Media.ImageSource)GetValue( SourceProperty ); }
			set { SetValue( SourceProperty, value ); }
		}

		internal ImageState State
		{
			get { return (ImageState)GetValue( StateProperty ); }
			set { base.SetValue( StateProperty, value ); }
		}

		public string Title
		{
			get { return (string)GetValue( TitleProperty ); }
			set { SetValue( TitleProperty, value ); }
		}
	}

	class VisibilityConverter : IValueConverter
	{
		// Methods
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (((bool) value) ^ ((bool) parameter))
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

	class MultipleToSingleLineStringConverter : IValueConverter
	{
		// Methods
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string str = value as string;
			if ( string.IsNullOrEmpty( str ) )
			{
				return string.Empty;
			}
			return str.Replace( Environment.NewLine, " " );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}

	class NavigationMenuRenderer : ViewRenderer<NavigationMenu, Grid>
	{
		protected override void OnElementChanged( ElementChangedEventArgs<NavigationMenu> e )
		{
			base.OnElementChanged( e );
			var grid = new Grid();
			grid.ColumnDefinitions.Add( new ColumnDefinition
			{
				Width = GridLength.Auto
			} );
			grid.ColumnDefinitions.Add( new ColumnDefinition
			{
				Width = GridLength.Auto
			} );
			UpdateItems( grid );
			SetNativeControl( grid );
		}

		protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged( sender, e );
			string propertyName;
			if ( ( propertyName = e.PropertyName ) != null )
			{
				if ( !( propertyName == "Targets" ) )
				{
					return;
				}
				UpdateItems( Control );
			}
		}

		TileSize GetSize()
		{
			if ( RenderSize.Width < 432.0 )
			{
				return TileSize.Default;
			}
			return TileSize.Medium;
		}

		void UpdateItems( Grid grid )
		{
			grid.Children.Clear();
			grid.RowDefinitions.Clear();
			var num = 0;
			var num2 = 0;
			foreach ( var current in Element.Targets )
			{
				if ( num > 1 )
				{
					num = 0;
					num2++;
				}
				if ( num == 0 )
				{
					grid.RowDefinitions.Add( new RowDefinition() );
				}
				var hubTile = new HubTile
				{
					Title = current.Title,
					Source = new BitmapImage( new Uri( current.Icon, UriKind.Relative ) ),
					Margin = new Thickness( 0.0, 0.0, 12.0, 12.0 )
				};
				if ( current.BackgroundColor != Color.Default )
				{
					hubTile.Background = current.BackgroundColor.ToBrush();
				}
				var tmp = current;
				hubTile.MouseLeftButtonUp += ( sender, args ) => Element.SendTargetSelected( tmp );
				hubTile.SetValue( Grid.RowProperty, num2 );
				hubTile.SetValue( Grid.ColumnProperty, num );
				hubTile.Size = GetSize();
				var weakRef = new WeakReference( hubTile );
				SizeChanged += delegate
				{
					if ( weakRef.IsAlive )
					{
						( (HubTile)weakRef.Target ).Size = this.GetSize();
					}
					( (IVisualElementController)this.Element ).NativeSizeChanged();
				};
				num++;
				grid.Children.Add( hubTile );
			}
		}
	}
}
