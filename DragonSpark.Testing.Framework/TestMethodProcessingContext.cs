using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Framework
{
	public class TestMethodProcessingContext
	{
		public TestMethodProcessingContext( IServiceLocator locator, Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext, MethodInfo testMethod )
		{
			Locator = locator;
			TestContext = testContext;
			TestMethod = testMethod;
		}

		public IServiceLocator Locator { get; private set; }
		public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext { get; private set; }
		public MethodInfo TestMethod { get; private set; }
	}
}