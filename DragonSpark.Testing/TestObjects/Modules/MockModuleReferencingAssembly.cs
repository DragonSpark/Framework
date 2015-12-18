

using DragonSpark.Modularity;

namespace DragonSpark.Testing.TestObjects.Modules
{
	public class MockModuleReferencingAssembly : IModule
    {
        public void Initialize()
        {
            MockReferencedModule instance = new MockReferencedModule();
        }

	    public void Load()
	    {
		    throw new System.NotImplementedException();
	    }
    }
}