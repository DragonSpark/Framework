using System;
using System.ComponentModel;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Objects.Synchronization;

namespace DragonSpark.Application.Presentation.Entity
{
	public class EntityTitleMonitor : ViewObject, IDisposable
	{
		readonly IEntitySetProfile entitySetProfile;
		readonly INotifyPropertyChanged source;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public EntityTitleMonitor( IEntitySetProfile entitySetProfile, INotifyPropertyChanged source )
		{
			this.entitySetProfile = entitySetProfile;
			this.source = source;
			source.PropertyChanged += XPropertyChanged;
			Update();
		}

		~EntityTitleMonitor()
		{
			Dispose( false );
		}

		void XPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == EntitySet.DisplayNamePath )
			{
				Update();
			}
		}

		void Update()
		{
			NotifyOfPropertyChange( () => Title );
		}

		public string Title
		{
			get
			{
				var result = source.EvaluateValue( EntitySet.DisplayNamePath ).Transform( y => y.As<string>() ?? y.ToString() );
				return result;
			}
		}

		public IEntitySetProfile EntitySet
		{
			get { return entitySetProfile; }
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			source.PropertyChanged -= XPropertyChanged;
		}
	}


	/*public interface IEntityViewModel
	{
		void Assign( IEntitySetProfile fieldEntitySet, ICollectionView view );
	}*/
}