using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity
{
	public class EntitySetCollectionViewSource : DomainCollectionViewSource
	{
		readonly IsolatedStorageSettings settings;
		readonly IDomainContextLocator locator;
		readonly IEntitySetService entitySetService;
		readonly IViewSupport viewSupport;

		public EntitySetCollectionViewSource( IsolatedStorageSettings settings, IDomainContextLocator locator, IEntitySetService entitySetService, IViewSupport viewSupport )
		{
			this.settings = settings;
			this.locator = locator;
			this.entitySetService = entitySetService;
			this.viewSupport = viewSupport;
		}

		void EntitySetsOnSelectionChanged( object o, EventArgs eventArgs )
		{
			UpdateSelected();
			settings.Set<SelectableCollection<IEntitySetProfile>, string>( x => x.SelectedItem.EntityType.FullName, EntityType.FullName );
			AssignView();
		}

		void UpdateSelected()
		{
			EntityType = EntitySets.SelectedItem.Transform( x => x.EntityType );
			DomainContext = locator.Locate( EntityType );
		}

		public IEntitySetService EntitySetService
		{
			get { return entitySetService; }
		}

		protected override void Initialize()
		{
			var profiles = ResolveProfiles();
			EntitySets.Reset( profiles );

			EntitySets.SelectedItem = DetermineSelected();
			EntitySets.SelectedItem.NotNull( x =>
			{
				UpdateSelected();
				base.Initialize();
			} );
			
			EntitySets.SelectionChanged += EntitySetsOnSelectionChanged;
			RefreshNotifications();
		}

		IEntitySetProfile DetermineSelected()
		{
			switch ( EntitySets.Count )
			{
				case 1:
					return EntitySets.Single();
				default:
					return settings.Get<SelectableCollection<IEntitySetProfile>,string>( x => x.SelectedItem.EntityType.FullName ).Transform( x => EntitySets.FirstOrDefault( y => y.EntityType.FullName == x ) );
			}
		}

		protected override IEnumerable<SortDescription> DetermineSortDescriptions( Type entityType )
		{
			var profile = EntitySets.FirstOrDefault( x => x.EntityType == entityType ).Transform( x => new SortDescription( x.DisplayNamePath, ListSortDirection.Ascending ).ToEnumerable() );
			var items = base.DetermineSortDescriptions( entityType ).ToArray();
			var result = items.Any() ? items : profile;
			return result;
		}

		public string ProfileViewName
		{
			get { return GetValue( ProfileViewNameProperty ).To<string>(); }
			set { SetValue( ProfileViewNameProperty, value ); }
		}	public static readonly DependencyProperty ProfileViewNameProperty = DependencyProperty.Register( "ProfileViewName", typeof(string), typeof(EntitySetCollectionViewSource), null );
		
		protected virtual IEnumerable<IEntitySetProfile> ResolveProfiles()
		{
			var result = entitySetService.RetrieveProfiles().Where( x => x.IsRoot ).ToArray();
			return result;
		}

		public EntityTitleMonitor TitleMonitor
		{
			get { return GetValue( TitleMonitorProperty ).To<EntityTitleMonitor>(); }
			private set { SetValue( TitleMonitorProperty, value ); }
		}	public static readonly DependencyProperty TitleMonitorProperty = DependencyProperty.Register( "TitleMonitor", typeof(EntityTitleMonitor), typeof(EntitySetCollectionViewSource), null );
	
		protected override Microsoft.Windows.Data.DomainServices.DomainCollectionView CreateView()
		{
			var result = base.CreateView();
			result.CurrentChanged += ( sender, args ) => RefreshNotifications();
			result.CollectionChanged += ( sender, args ) =>
			{
			    switch ( args.Action )
			    {
			        case NotifyCollectionChangedAction.Add:
			        case NotifyCollectionChangedAction.Remove:
			            RefreshNotifications();
						break;
			    }
			};
			return result;
		}

		protected override void OnLoadComplete( EventArgs args )
		{
			base.OnLoadComplete( args );
			RefreshNotifications();
		}

		public void RefreshNotifications()
		{
			CommandButtonsVisibility = DefaultButtons | viewSupport.DetermineVisibility( DomainContext, EntityType );
			
			EntitySets.SelectedItem.NotNull( x => View.NotNull( y => y.CurrentItem.As<INotifyPropertyChanged>( z =>
			{
			    TitleMonitor = new EntityTitleMonitor( x, z );
			} ) ) );
		}

		public DataFormCommandButtonsVisibility CommandButtonsVisibility
		{
			get { return GetValue( CommandButtonsVisibilityProperty ).To<DataFormCommandButtonsVisibility>(); }
			private set { SetValue( CommandButtonsVisibilityProperty, value ); }
		}	public static readonly DependencyProperty CommandButtonsVisibilityProperty = DependencyProperty.Register( "CommandButtonsVisibility", typeof(DataFormCommandButtonsVisibility), typeof(EntitySetCollectionViewSource), null );
		
		public DataFormCommandButtonsVisibility DefaultButtons
		{
			get { return GetValue( DefaultButtonsProperty ).To<DataFormCommandButtonsVisibility>(); }
			set { SetValue( DefaultButtonsProperty, value ); }
		}	public static readonly DependencyProperty DefaultButtonsProperty = DependencyProperty.Register( "DefaultButtons", typeof(DataFormCommandButtonsVisibility), typeof(EntitySetCollectionViewSource), new PropertyMetadata( DataFormCommandButtonsVisibility.Cancel | DataFormCommandButtonsVisibility.Edit ) );

		public SelectableCollection<IEntitySetProfile> EntitySets
		{
			get { return entitySets; }
		}	readonly SelectableCollection<IEntitySetProfile> entitySets = new SelectableCollection<IEntitySetProfile>();

		public string Query
		{
			get { return GetValue( QueryProperty ).To<string>(); }
			set { SetValue( QueryProperty, value ); }
		}	public static readonly DependencyProperty QueryProperty = DependencyProperty.Register( "Query", typeof(string), typeof(EntitySetCollectionViewSource), null );

		public string NoEntitySetsFoundMessage
		{
			get { return GetValue( NoEntitySetsFoundMessageProperty ).To<string>(); }
			set { SetValue( NoEntitySetsFoundMessageProperty, value ); }
		}	public static readonly DependencyProperty NoEntitySetsFoundMessageProperty = DependencyProperty.Register( "NoEntitySetsFoundMessage", typeof(string), typeof(EntitySetCollectionViewSource), new PropertyMetadata( "No Entity Sets Were Found for this Application." ) );

		public string NoItemsFoundMessage
		{
			get { return GetValue( NoItemsFoundMessageProperty ).To<string>(); }
			set { SetValue( NoItemsFoundMessageProperty, value ); }
		}	public static readonly DependencyProperty NoItemsFoundMessageProperty = DependencyProperty.Register( "NoItemsFoundMessage", typeof(string), typeof(EntitySetCollectionViewSource), new PropertyMetadata( "No items were found." ) );

		public string NoItemsFoundInSearchMessage
		{
			get { return GetValue( NoItemsFoundInSearchMessageProperty ).To<string>(); }
			set { SetValue( NoItemsFoundInSearchMessageProperty, value ); }
		}	public static readonly DependencyProperty NoItemsFoundInSearchMessageProperty = DependencyProperty.Register( "NoItemsFoundInSearchMessage", typeof(string), typeof(EntitySetCollectionViewSource), new PropertyMetadata( "No items found that meet the specified criteria." ) );
	}
}