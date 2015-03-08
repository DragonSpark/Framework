using System.Data.Entity;

namespace DragonSpark.Application.Communication.Entity
{
	public class DropCreateDatabaseAlways<TContext> : System.Data.Entity.DropCreateDatabaseAlways<TContext> where TContext : DbContext, IEntityStorage
	{
		protected override void Seed( TContext context )
		{
			context.Install();
		}
	}
}