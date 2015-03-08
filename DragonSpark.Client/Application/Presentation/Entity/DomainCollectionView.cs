using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using DragonSpark.Extensions;
using Microsoft.Windows.Data.DomainServices;

namespace DragonSpark.Application.Presentation.Entity
{
	public class DomainCollectionView<TEntity> : Microsoft.Windows.Data.DomainServices.DomainCollectionView<TEntity>, IAppendSource, ILoadOperationListener where TEntity : System.ServiceModel.DomainServices.Client.Entity
	{
		readonly EntityList<TEntity> source;
		readonly IEnumerable<TEntity> originalSource;

		public DomainCollectionView( DomainCollectionViewLoader collectionViewLoader, EntityList<TEntity> source ) : base( collectionViewLoader, source )
		{
			this.source = source;
			originalSource = source.Source;

			SetTotalItemCount( originalSource.Count() );
		}

		public override bool Contains( object item )
		{
			var result = base.Contains( item ) || ( IsAddingNew && item.AsTo<TEntity,bool>( source.Contains ) );
			return result;
		}

		public int PageCount
		{
			get { return (int)Math.Ceiling( (double)TotalItemCount / PageSize ); }
		}

		void ILoadOperationListener.OnLoad( LoadOperation operation )
		{
			if ( operation.IsCanceled || operation.TotalEntityCount > -1 )
			{
				SetTotalItemCount( operation.IsCanceled ? source.Source.Count() : operation.TotalEntityCount );
				RaisePropertyChanged( "PageCount" );
			}

			source.Source = operation.IsCanceled ? originalSource : operation.Entities.OfType<TEntity>().ToArray();

			CurrentItem.Null( () => this.Any().IsTrue( () => MoveCurrentToFirst() ) );

			switch ( PageIndex )
			{
				case -1:
					MoveToFirstPage();
					break;
			}
		}

		void IAppendSource.Append( IEnumerable<object> items )
		{
			source.Source = source.Source.Concat( items.OfType<TEntity>() ).ToArray();
		}
	}
}