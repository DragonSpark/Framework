

using DragonSpark.Modularity;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
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