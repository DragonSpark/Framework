

using DragonSpark.Modularity;
using DragonSpark.Windows.Modularity;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
{
	[DynamicModule(ModuleName = "TestModule", OnDemand = true)]
	public class MockAttributedModule : IModule
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
