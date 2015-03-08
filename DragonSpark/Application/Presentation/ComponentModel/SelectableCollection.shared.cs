using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public class SelectableCollection<TItem> : ViewCollection<TItem>
	{
		public event EventHandler<SelectionChangingEventArgs<TItem>> SelectionChanging = delegate { };
		public event EventHandler SelectionChanged = delegate { };

		public SelectableCollection()
		{}

        public SelectableCollection( IEnumerable<TItem> items ) : base( items )
		{}

		public void Reset( IEnumerable<TItem> items )
		{
			var contains = items.Contains( SelectedItem );
			var assign = Assign( default(TItem) );
			if ( contains || assign )
			{
				Clear();
				AddRange( items );
			}
		}

		public TItem SelectedItem
		{
			get { return selectedItem; }
			set
			{
				value = ResolveValue( value );
				Assign( value );
			}
		}

		TItem ResolveValue( TItem value )
		{
			return Contains( value ) ? value : default( TItem );
		}

		bool Assign( TItem value )
		{
			if ( !Equals( value, selectedItem ) )
			{
				var args = new SelectionChangingEventArgs<TItem>( selectedItem, value );
				OnChanging( args );
				var result = !args.Cancel;
				if ( result )
				{
					selectedItem = value;
					OnChanged( EventArgs.Empty );
				}
				IsNotifying.IsTrue( () => NotifyOfPropertyChange( "SelectedItem" ) );
				return result;
			}
			return true;
		}	TItem selectedItem;

		protected virtual void OnChanged( EventArgs args )
		{
			SelectionChanged( this, args );
		}

		protected virtual void OnChanging( SelectionChangingEventArgs<TItem> args )
		{
			SelectionChanging( this, args );
		}
	}
}