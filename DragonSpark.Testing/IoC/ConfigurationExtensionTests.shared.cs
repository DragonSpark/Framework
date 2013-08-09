using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects.IoC;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.IoC
{
	/// <summary>
	/// Summary description for ConfigurationExtensionTests
	/// </summary>
	[TestClass]
	public class ConfigurationExtensionTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifySpyPolicyCreatedProperly()
		{
			var policy = new Configuration.Configuration().Instance.Instance.GetInstance<SpyPolicy>();
			Assert.IsTrue( policy.Enabled );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifySpyStrategyConfiguredCorrectly()
		{
			var target = new Configuration.Configuration().Instance.Instance.GetInstance<ISpyTarget>();
			Assert.IsTrue( target.WasSpiedOn );
		}
	}
}
