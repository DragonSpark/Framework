using System.Data.Entity;
using DragonSpark.Activation;

namespace DragonSpark.Server.Legacy.Entity
{
	public static class EntityInstallationInitialization
	{
		public static void Install<TContext>( this TContext context ) where TContext : DbContext, IEntityStorage
		{
			var installation = Activator.Create<InstallationContext>();
			installation.Install( context );
		}
	}
}