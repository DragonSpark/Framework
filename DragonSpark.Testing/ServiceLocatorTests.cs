using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Objects;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing
{
	[TestClass]
	public class ServiceLocatorTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyDefaultLocatorWorks()
		{
			ServiceLocator.SetLocatorProvider( () => new ApplicationConfiguration().Instance );

			var service = ServiceLocator.Current.GetInstance<DragonSparkObject>( "DefaultKey" );
			Assert.IsNotNull( service );
			Assert.AreEqual( "Default Name", service.Name );
		}
	}
}