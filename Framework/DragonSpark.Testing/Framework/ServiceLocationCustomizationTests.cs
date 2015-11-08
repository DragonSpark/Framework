using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Framework
{
	public class ServiceLocationCustomizationTests
	{
		[Theory, AutoDataCustomization, AssignServiceLocation]
		public void GetAllInstances( IServiceLocator sut )
		{
			Assert.True( sut.GetAllInstances<Class>().Any() );
		}
	}
}