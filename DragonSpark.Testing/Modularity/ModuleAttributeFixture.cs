using DragonSpark.Modularity;
using DragonSpark.Windows.Modularity;
using Xunit;

namespace DragonSpark.Testing.Modularity
{
	public class ModuleAttributeFixture
	{
		public void StartupLoadedDefaultsToTrue()
		{
			var moduleAttribute = new DynamicModuleAttribute();

			Assert.Equal(false, moduleAttribute.OnDemand);
		}

		public void CanGetAndSetProperties()
		{
			var moduleAttribute = new DynamicModuleAttribute
			{
				ModuleName = "Test",
				OnDemand = true
			};

			Assert.Equal("Test", moduleAttribute.ModuleName);
			Assert.Equal(true, moduleAttribute.OnDemand);
		}

		public void ModuleDependencyAttributeStoresModuleName()
		{
			var moduleDependencyAttribute = new ModuleDependencyAttribute("Test");

			Assert.Equal("Test", moduleDependencyAttribute.ModuleName);
		}
	}
}