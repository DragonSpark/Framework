

using DragonSpark.Modularity;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
{
	public abstract class MockAbstractModule : IModule
	{
		public void Initialize()
		{
		}

		public void Load()
		{
			throw new System.NotImplementedException();
		}
	}

	public class MockInheritingModule : MockAbstractModule
	{}
}
