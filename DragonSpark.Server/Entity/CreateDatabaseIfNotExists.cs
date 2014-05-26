using System.Data.Entity;

namespace DragonSpark.Entity
{
    public class CreateDatabaseIfNotExists<TContext> : System.Data.Entity.CreateDatabaseIfNotExists<TContext> where TContext : DbContext, IEntityStorage
    {
        protected override void Seed( TContext context )
        {
            context.Install();
        }
    }
}