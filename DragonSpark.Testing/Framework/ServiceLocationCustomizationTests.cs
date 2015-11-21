using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Framework
{
	public class ServiceLocationCustomizationTests
	{
		[Theory, AutoData( typeof(Testing.Customizations.Assigned) )]
		public void GetAllInstances( IServiceLocator sut )
		{
			Assert.True( sut.GetAllInstances<Class>().Any() );
		}
	}
}