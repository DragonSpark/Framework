using DragonSpark.Windows.Modularity;
using Xunit;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class ConfigurationStoreTests
	{
		[Fact]
		public void ShouldRetrieveModuleConfiguration()
		{
			ConfigurationStore store = new ConfigurationStore();
			var section = store.RetrieveModuleConfigurationSection();

			Assert.NotNull(section);
			Assert.NotNull(section.Modules);
			Assert.Equal(1, section.Modules.Count);
			var element = section.Modules[0];
			Assert.NotNull(element.AssemblyFile);
			Assert.Equal("MockModuleA", element.ModuleName);
			Assert.NotNull(element.AssemblyFile);
			Assert.True(element.AssemblyFile.Contains(@"MocksModules\MockModuleA.dll"));
			Assert.NotNull(element.ModuleType);
			Assert.True(element.StartupLoaded);
			Assert.Equal("DragonSpark.Windows.Modularity.MockModuleA", element.ModuleType);
		}
	}
}