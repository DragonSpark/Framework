

using DragonSpark.Modularity;

namespace DragonSpark.Testing.TestObjects.Modules
{
	[Module(ModuleName = "DependencyModule")]
	public class DependencyModule : IModule
	{
		public void Initialize()
		{
			throw new System.NotImplementedException();
		}

		public void Load()
		{
			throw new System.NotImplementedException();
		}
	}
}
