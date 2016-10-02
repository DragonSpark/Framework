using System.Data.Entity;

namespace DragonSpark.Windows.Entity
{
	public struct DbContextBuildingParameter
	{
		public DbContextBuildingParameter( DbContext context, DbModelBuilder builder )
		{
			Context = context;
			Builder = builder;
		}

		public DbContext Context { get; }
		public DbModelBuilder Builder { get; }
	}
}