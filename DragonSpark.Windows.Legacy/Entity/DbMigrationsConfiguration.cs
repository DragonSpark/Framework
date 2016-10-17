using DragonSpark.Aspects;
using System.Data.Entity;

namespace DragonSpark.Windows.Legacy.Entity
{
	[ApplyValuesFromSource]
	public class DbMigrationsConfiguration<T> : System.Data.Entity.Migrations.DbMigrationsConfiguration<T> where T : DbContext {}
}