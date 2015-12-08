using DragonSpark.Windows.Modularity;
using Xunit;

namespace DragonSpark.Testing.Modularity
{
	public class ModuleInfoGroupFixture
	{
		public void ShouldForwardValuesToModuleInfo()
		{
			var group = new DynamicModuleInfoGroup();
			group.Ref = "MyCustomGroupRef";
			var moduleInfo = new DynamicModuleInfo();
			Assert.Null(moduleInfo.Ref);

			group.Add(moduleInfo);

			Assert.Equal(group.Ref, moduleInfo.Ref);
		}
	}
}
