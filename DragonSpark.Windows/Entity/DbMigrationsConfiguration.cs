using System.Data.Entity;

namespace DragonSpark.Windows.Entity
{
	public class DbMigrationsConfiguration<TContext> : System.Data.Entity.Migrations.DbMigrationsConfiguration<TContext> where TContext : DbContext
	{
		public DbMigrationsConfiguration() : this( ActivationSource.Instance )
		{}

		public DbMigrationsConfiguration( IActivationSource source )
		{
			source.Apply( this );
		}
	}
}