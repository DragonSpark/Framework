using System;
using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Presentation.Entity
{
	public class DomainCollectionViewLoader : Microsoft.Windows.Data.DomainServices.DomainCollectionViewLoader
	{
		public event EventHandler Loading = delegate { };

		public DomainCollectionViewLoader( Func<LoadOperation> load ) : base( load )
		{}

		public DomainCollectionViewLoader( Func<LoadOperation> load, Action<LoadOperation> onLoadCompleted ) : base( load, onLoadCompleted )
		{}

		public override void Load( object userState )
		{
			Loading( this, EventArgs.Empty );
			base.Load( userState );
		}
	}
}