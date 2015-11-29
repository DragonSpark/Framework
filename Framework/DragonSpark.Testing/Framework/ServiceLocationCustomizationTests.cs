using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Framework
{
	public class ServiceLocationCustomizationTests
	{
		[Theory, Test, SetupAutoData]
		public void GetAllInstances( IServiceLocator sut )
		{
			Assert.False( sut.GetAllInstances<Class>().Any() );
		}
	}
}