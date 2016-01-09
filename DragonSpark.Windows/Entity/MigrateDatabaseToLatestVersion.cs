using System.Data.Entity;
using DragonSpark.Activation;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Windows.Entity
{
	public class MigrateDatabaseToLatestVersion<TContext, TConfiguration> : System.Data.Entity.MigrateDatabaseToLatestVersion<TContext, TConfiguration> where TContext : DbContext where TConfiguration : DbMigrationsConfiguration<TContext>, new()
	{
		public MigrateDatabaseToLatestVersion() : this( false )
		{}

		public MigrateDatabaseToLatestVersion( bool useSuppliedContext ) : base( useSuppliedContext, Activator.GetCurrent().Activate<TConfiguration>() )
		{}
	}
}