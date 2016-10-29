using DragonSpark.Activation.Location;
using DragonSpark.Extensions;
using System.Composition;
using System.Data.Entity;

namespace DragonSpark.Windows.Legacy.Entity
{
	public class MigrateDatabaseToLatestVersion<TContext, TConfiguration> : System.Data.Entity.MigrateDatabaseToLatestVersion<TContext, TConfiguration> where TContext : DbContext where TConfiguration : DbMigrationsConfiguration<TContext>, new()
	{
		[ImportingConstructor]
		public MigrateDatabaseToLatestVersion() : this( false ) {}

		public MigrateDatabaseToLatestVersion( bool useSuppliedContext ) : base( useSuppliedContext, GlobalServiceProvider.Default.Get<TConfiguration>() ) {}
	}
}