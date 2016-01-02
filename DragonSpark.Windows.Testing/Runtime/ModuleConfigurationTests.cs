using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Modularity;
using DragonSpark.Windows.Testing.TestObjects;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ModuleConfigurationTests
	{
		[Theory, MoqAutoData]
		public void Load( ModulesConfiguration sut )
		{
			var section = new ModulesConfigurationSectionFactory( sut ).Create();
			Assert.True( section.Modules.Count > 0 );
			Assert.True( section.Modules[0].Dependencies.Count > 0 );
			Assert.NotNull( section.Modules[0].Dependencies[0]  );
		}
	}
}