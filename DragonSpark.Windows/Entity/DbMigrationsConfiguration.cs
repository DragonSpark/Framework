using DragonSpark.Aspects;
using System.Data.Entity;

namespace DragonSpark.Windows.Entity
{
	[ApplyValuesFromSource]
	public class DbMigrationsConfiguration<T> : System.Data.Entity.Migrations.DbMigrationsConfiguration<T> where T : DbContext {}
}