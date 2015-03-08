using System.Linq;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class InclusiveItemViewModel<TItem> : ViewModel<TItem> where TItem : class
	{
		readonly IObservableCollection<TItem> trackingCollection;

		public InclusiveItemViewModel( IObservableCollection<TItem> trackingCollection, TItem source ) : base( source )
		{
			this.trackingCollection = trackingCollection;
			trackingCollection.CollectionChanged += ( sender, args ) => args.AllItems<TItem>().Contains( Source ).IsTrue( Update );
			Update();
		}

		void Update()
		{
			IsIncluded = trackingCollection.Contains( Source );
		}
		
		public bool IsIncluded
		{
			get { return isIncluded; }
			set
			{
				
				if ( isIncluded != value )
				{
					isIncluded = value;

					UpdateMembership( value );

					NotifyOfPropertyChange( () => IsIncluded );
				}
			}
		}	bool isIncluded;
		
		void UpdateMembership( bool value )
		{
			if ( value && !trackingCollection.Contains( Source ) )
			{
				trackingCollection.Add( Source );
			}
			else if ( !value && trackingCollection.Contains( Source ) )
			{
				trackingCollection.Remove( Source );
			}
		}
	}
}