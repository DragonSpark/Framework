using System.Linq;
using DragonSpark.Testing.Framework.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Framework.Testing
{
	public class ServiceLocationCustomizationTests
	{
		[Theory, AutoData, AssignServiceLocation]
		public void GetAllInstances( IServiceLocator sut )
		{
			Assert.True( sut.GetAllInstances<Class>().Any() );
		}
	}
}