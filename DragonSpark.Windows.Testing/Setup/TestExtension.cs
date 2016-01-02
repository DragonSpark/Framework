using Microsoft.Practices.Unity;

namespace DragonSpark.Windows.Testing.Setup
{
	public class TestExtension : UnityContainerExtension
	{
		public bool Initialized { get; private set; }

		protected override void Initialize()
		{
			Initialized = true;
		}
	}
}