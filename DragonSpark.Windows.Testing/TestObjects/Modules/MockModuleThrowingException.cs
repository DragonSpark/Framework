

using DragonSpark.Modularity;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
{
    public class MockModuleThrowingException : IModule
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
