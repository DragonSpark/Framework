using System.Data.Entity;

namespace DragonSpark.Entity
{
    public class DropCreateDatabaseIfModelChanges<TContext> : System.Data.Entity.DropCreateDatabaseIfModelChanges<TContext> where TContext : DbContext, IEntityStorage
    {
        protected override void Seed( TContext context )
        {
            context.Install();
        }
    }
}