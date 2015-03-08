using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using Microsoft.Windows.Data.DomainServices;

namespace DragonSpark.Application.Presentation.Entity
{
	public class DomainCollectionViewSource : DependencyObject, ISupportInitialize
	{
		public event EventHandler<LoadOperationEventArgs> Loading = delegate { };
		public event EventHandler LoadComplete = delegate { }, Ready = delegate {  };

		readonly BitFlipper ready = new BitFlipper();
		readonly IDictionary<string, DomainCollectionView> views = new Dictionary<string, DomainCollectionView>();

		static readonly MethodInfo CreateMethod = typeof(DomainCollectionViewSource).GetMethod( "Create", DragonSparkBindingOptions.AllProperties );
		
		[Required]
		public DomainContext DomainContext
		{
			get { return GetValue( DomainContextProperty ).To<DomainContext>(); }
			set { SetValue( DomainContextProperty, value ); }
		}	public static readonly DependencyProperty DomainContextProperty = DependencyProperty.Register( "DomainContext", typeof(DomainContext), typeof(DomainCollectionViewSource), new PropertyMetadata( OnUpdate ) );

		static void OnUpdate( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.As<DomainCollectionViewSource>( x => x.CheckUpdate() );
		}

		[Required]
		public Type EntityType
		{
			get { return GetValue( EntityTypeProperty ).To<Type>(); }
			set { SetValue( EntityTypeProperty, value ); }
		}	public static readonly DependencyProperty EntityTypeProperty = DependencyProperty.Register( "EntityType", typeof(Type), typeof(DomainCollectionViewSource), new PropertyMetadata( OnUpdate ) );

		public IEnumerable InitialItems
		{
			get { return GetValue( InitialItemsProperty ).To<IEnumerable>(); }
			set { SetValue( InitialItemsProperty, value ); }
		}	public static readonly DependencyProperty InitialItemsProperty = DependencyProperty.Register( "InitialItems", typeof(IEnumerable), typeof(DomainCollectionViewSource), new PropertyMetadata( Enumerable.Empty<object>(), OnUpdate ) );

		void CheckUpdate()
		{
			var assign = Parsed && Initialized && this.Validate() && View == null;
			assign.IsTrue( AssignView );
		}

		protected void AssignView()
		{
			var key = string.Concat( DomainContext.GetType().AssemblyQualifiedName, EntityType.AssemblyQualifiedName );
			View = views.Ensure( key, x => CreateView() );
			ready.Check( () => Threading.Application.Start( () => Ready( this, EventArgs.Empty ) ) );
		}

		public bool EnableAutomaticLoading
		{
			get { return GetValue( EnableAutomaticLoadingProperty ).To<bool>(); }
			set { SetValue( EnableAutomaticLoadingProperty, value ); }
		}	public static readonly DependencyProperty EnableAutomaticLoadingProperty = DependencyProperty.Register( "EnableAutomaticLoading", typeof(bool), typeof(DomainCollectionViewSource), new PropertyMetadata( true ) );

		public DomainCollectionView View
		{
			get { return GetValue( ViewProperty ).To<DomainCollectionView>(); }
			private set { SetValue( ViewProperty, value ); }
		}	public static readonly DependencyProperty ViewProperty = DependencyProperty.Register( "View", typeof(DomainCollectionView), typeof(DomainCollectionViewSource), new PropertyMetadata( OnViewChanged ) );

		static void OnViewChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.As<DomainCollectionViewSource>( x => x.EnableAutomaticLoading.IsTrue( () =>
			{
				var call = x.ready.Flipped ? (Func<Action, IDelegateContext>)Threading.Application.Execute : Threading.Application.Start;
				call( () => x.View.Reload() );
			} ) );
		}

		public int PageSize
		{
			get { return GetValue( PageSizeProperty ).To<int>(); }
			set { SetValue( PageSizeProperty, value ); }
		}	public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register( "PageSize", typeof(int), typeof(DomainCollectionViewSource), new PropertyMetadata( 10, ( s, a ) => s.As<DomainCollectionViewSource>( x => x.View.NotNull( y => y.PageSize = a.NewValue.To<int>() ) ) ) );

		/*public int PageSize
		{
			get { return pageSize; }
			set { SetProperty( ref pageSize, value, () => PageSize ).IsTrue( () => View.NotNull( x => x.PageSize = value ) ); }
		}	int pageSize = 10;*/
		
		protected virtual DomainCollectionView CreateView()
		{
			var baseType = DomainContext.DetermineEntitySet( EntityType ).EntityType;
			var result = CreateMethod.MakeGenericMethod( baseType, EntityType ).Invoke( this, null ).To<DomainCollectionView>();
			return result;
		}

		
		protected virtual IEnumerable<SortDescription> DetermineSortDescriptions( Type entityType )
		{
			var result = entityType.GetAttributes<DisplayColumnAttribute>().Select( x => new SortDescription( x.SortColumn ?? x.DisplayColumn, x.SortDescending ? ListSortDirection.Descending : ListSortDirection.Ascending ) );
			return result;
		}

		DomainCollectionView<TBase> Create<TBase,TEntity>() where TEntity : System.ServiceModel.DomainServices.Client.Entity where TBase : System.ServiceModel.DomainServices.Client.Entity
		{
			var entitySet = DomainContext.EntityContainer.GetEntitySet<TBase>();
			
			var entities = InitialItems.Cast<TEntity>() ?? entitySet.OfType<TEntity>();
			var source = new EntityList<TBase>( entitySet, entities.OfType<TBase>().ToArray() );

			var loader = new DomainCollectionViewLoader( Load, LoadCompleted );
			var result = new DomainCollectionView<TBase>( loader, source ) { PageSize = PageSize };
			result.SortDescriptions.AddAll( DetermineSortDescriptions( typeof(TEntity) ) );
			return result;
		}

		LoadOperation Load()
		{
			var args = new LoadOperationEventArgs();
			Loading( this, args );
			var result = args.Operation;
			return result;
		}

		void LoadCompleted( LoadOperation operation )
		{
			operation.HasError.IsTrue( operation.MarkErrorAsHandled );

			View.As<ILoadOperationListener>( x => x.OnLoad( operation ) );

			OnLoadComplete( EventArgs.Empty );
		}

		protected virtual void OnLoadComplete( EventArgs args )
		{
			LoadComplete( this, args );
		}

		bool Initialized { get; set; }
		bool Parsed { get; set; }

		void ISupportInitialize.BeginInit()
		{}

		void ISupportInitialize.EndInit()
		{
			Parsed = true;
			Initialize();
		}

		protected virtual void Initialize()
		{
			Threading.Application.Start( () =>
			{
				Initialized = true;
				CheckUpdate();
			} );
		}
	}
}