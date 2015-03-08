using System;
using System.ComponentModel;
using System.Windows.Controls;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class ManageEntityCollectionContext : ViewObject, IDisposable
	{
		readonly ICollectionView view;
		readonly IEntitySetProfile profile;
		readonly DataFormCommandButtonsVisibility visibility;
		readonly string viewProfileName;

		public ManageEntityCollectionContext( ICollectionView view, IEntitySetProfile profile, DataFormCommandButtonsVisibility visibility, string viewProfileName )
		{
			this.view = view;
			this.profile = profile;
			this.visibility = visibility;
			this.viewProfileName = viewProfileName;

			view.CurrentChanged += ViewCurrentChanged;
		}

		void ViewCurrentChanged( object sender, EventArgs e )
		{
			Update();
		}

		public void Initialize()
		{
			Update();
		}

		void Update()
		{
			view.CurrentItem.As<INotifyPropertyChanged>( z => EntityTitle = new EntityTitleMonitor( profile, z ) );
		}

		public ICollectionView View
		{
			get { return view; }
		}

		public IEntitySetProfile Profile
		{
			get { return profile; }
		}

		public DataFormCommandButtonsVisibility Visibility
		{
			get { return visibility; }
		}

		public string ViewProfileName
		{
			get { return viewProfileName; }
		}

		public EntityTitleMonitor EntityTitle
		{
			get { return entityTitle; }
			private set
			{
				if ( entityTitle != value )
				{
					entityTitle.NotNull( x => this.StopWatching( x, () => EntityTitle ).Dispose() );
					entityTitle = this.Watching( value, () => EntityTitle );
				}
			}
		}	EntityTitleMonitor entityTitle;

		public bool Result
		{
			get { return result; }
			set { SetProperty( ref result, value, () => Result ); }
		}	bool result;
		
		public void Dispose()
		{
			view.CurrentChanged -= ViewCurrentChanged;
		}
	}
}