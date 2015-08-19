

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Modularity;

namespace Prism.Wpf.Tests.Modularity
{
    [TestClass]
    public class ModuleInfoGroupFixture
    {
        [TestMethod]
        public void ShouldForwardValuesToModuleInfo()
        {
            var group = new DynamicModuleInfoGroup();
            group.Ref = "MyCustomGroupRef";
            var moduleInfo = new DynamicModuleInfo();
            Assert.IsNull(moduleInfo.Ref);

            group.Add(moduleInfo);

            Assert.AreEqual(group.Ref, moduleInfo.Ref);
        }
    }
}
