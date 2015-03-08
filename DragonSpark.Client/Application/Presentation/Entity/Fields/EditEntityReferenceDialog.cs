using System;
using System.ComponentModel;
using System.Windows;
using DragonSpark.Application.Presentation.ComponentModel;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public abstract class EditEntityReferenceDialog : ViewObject
	{
		protected EditEntityReferenceDialog( ICollectionView view, Action<MessageBoxResult> onResult ) // : base( onResult )
		{
			View = view;
		}

		public ICollectionView View
		{
			get { return view; }
			private set
			{
				if ( view != value )
				{
					view = value;
					NotifyOfPropertyChange( () => View );
				}
			}
		}	ICollectionView view;
	}
}