using System.Data.Entity;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Features.Entity;

namespace DragonSpark.Features
{
	public class FeaturesEntityStorage : EntityStorage<User>
	{
		public DbSet<SampleUser> SampleUsers { get; set; }

		public DbSet<BasicEntity> BasicEntities { get; set; }

		public DbSet<EntityWithReference> EntityWithReferences { get; set; }
		public DbSet<ReferencedEntity> ReferencedEntities { get; set; }

		public DbSet<EntityWithCollection> EntityWithCollections { get; set; }
		public DbSet<CollectionEntity> CollectionEntities { get; set; }
	}
}