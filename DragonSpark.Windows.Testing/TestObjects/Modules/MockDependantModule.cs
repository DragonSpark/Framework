

using DragonSpark.Modularity;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
{
	[Module(ModuleName = "DependantModule")]
	[ModuleDependency("DependencyModule")]
	public class DependantModule : IModule
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
