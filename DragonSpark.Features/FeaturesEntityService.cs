using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Features.Entity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace DragonSpark.Features
{
    [EnableClientAccess, ExceptionShielding]
    public class FeaturesEntityService : DbDomainService<FeaturesEntityStorage>
    {
        protected override bool PersistChangeSet()
        {
            SaveChanges();
            return true;
        }

        [Conditional( "DEBUG" )]
        void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        // -- SampleUser --

        public void UpdateUser( SampleUser user )
        {
            DbContext.ApplyChanges( user );
        }

        public IQueryable<SampleUser> RetrieveSampleUsers( string filter )
        {
            var result = DbContext.Set<SampleUser>().Query( filter );
            return result;
        }
		
        // -- BasicEntity --

        public void Add( BasicEntity entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Update( BasicEntity entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Remove( BasicEntity entity )
        {
            DbContext.Remove( entity );
        }

        public IQueryable<BasicEntity> RetrieveBasicEntities( string filter )
        {
            var result = DbContext.Set<BasicEntity>().Query( filter );
            return result;
        }

        // -- EntityWithReference --

        public void Add( EntityWithReference entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Update( EntityWithReference entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Remove( EntityWithReference entity )
        {
            DbContext.Remove( entity );
        }

        public IQueryable<EntityWithReference> RetrieveEntityWithReferences( string filter )
        {
            var result = DbContext.Set<EntityWithReference>().Query( filter );
            return result;
        }

        // -- ReferencedEntity --
        public void Add( ReferencedEntity entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Update( ReferencedEntity entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Remove( ReferencedEntity entity )
        {
            DbContext.Remove( entity );
        }

        public IQueryable<ReferencedEntity> RetrieveReferencedEntities( string filter )
        {
            var result = DbContext.Set<ReferencedEntity>().Query( filter );
            return result;
        }

        // -- EntityWithCollection --

        public void Add( EntityWithCollection entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Update( EntityWithCollection entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Remove( EntityWithCollection entity )
        {
            DbContext.Remove( entity );
        }

        public IQueryable<EntityWithCollection> RetrieveEntityWithCollections( string filter )
        {
            var result = DbContext.Set<EntityWithCollection>().Include( x => x.Entities ).ToArray().AsQueryable().Query( filter );
            return result;
        }

        // -- CollectionEntity --

        public void Add( CollectionEntity entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Update( CollectionEntity entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Remove( CollectionEntity entity )
        {
            DbContext.Remove( entity );
        }

        public IQueryable<CollectionEntity> RetrieveCollectionEntities( string filter )
        {
            var result = DbContext.Set<CollectionEntity>().Query( filter );
            return result;
        }
    }
}