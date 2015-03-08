using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public abstract class RolesFieldBase<TRole> : ViewObject where TRole : class, IObjectWithName
	{
		readonly ViewCollection<TRole> trackingItems = new ViewCollection<TRole>();
		
		protected RolesFieldBase()
		{
			trackingItems.CollectionChanged += TrackingItemsUpdated;
		}

		void TrackingItemsUpdated( object sender, NotifyCollectionChangedEventArgs e )
		{
			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Remove:
					Value = string.Join( ";", trackingItems.Select( x => x.Name ) );
					break;
			}
		}
		
		public void Refresh()
		{
			this.Validate().IsTrue( () =>
			{
				var items = Value.ToStringArray().Select( y => View.SourceCollection.OfType<TRole>().FirstOrDefault( z => z.Name == y ) ).NotNull().ToArray();
				trackingItems.Reset( items, TrackingItemsUpdated );
			} );
		}

		[Required]
		public ICollectionView View
		{
			get { return view; }
			set
			{
				if ( SetProperty( ref view, value, () => View ) )
				{
					SourceItems = new ViewModelCollectionView<TRole,InclusiveItemViewModel<TRole>>( value, x => new InclusiveItemViewModel<TRole>( trackingItems, x ) );
				}
			}
		}	ICollectionView view;

		public string Value
		{
			get { return valueField; }
			set { SetProperty( ref valueField, value, () => Value ).IsTrue( Refresh ); }
		}	string valueField;

		public IObservableCollection<InclusiveItemViewModel<TRole>> SourceItems
		{
			get { return sourceItems; }
			private set { SetProperty( ref sourceItems, value, () => SourceItems ); }
		}	IObservableCollection<InclusiveItemViewModel<TRole>> sourceItems;
	}
}
