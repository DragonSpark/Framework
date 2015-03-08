using System.Data.Entity;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Application.Communication.Entity
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