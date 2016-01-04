using DragonSpark.Modularity;
using DragonSpark.Testing.Framework.Setup;
using Xunit;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class ModuleInfoBuilderTests
	{
		[Theory, ConfiguredMoqAutoData]
		public void Create( ModuleInfoBuilder sut )
		{
			var module = sut.CreateModuleInfo( typeof(Module) );
			Assert.NotNull( module );
		}

		[Module]
		public class Module : IModule
		{
			public bool Initialized { get; private set; }
			
			public void Initialize()
			{
				Initialized = true;
			}
		}
	}
}