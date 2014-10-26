using System.Reflection;
using DragonSpark.Activation;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Xunit;

namespace DragonSpark.Testing.Framework
{
	public class AssignServiceLocationAttribute : BeforeAfterTestAttribute
	{
		public override void Before( MethodInfo methodUnderTest )
		{
			base.Before( methodUnderTest );

			var serviceLocator = Fixture.Current.Create<IServiceLocator>();
			ServiceLocation.Assign( serviceLocator );
		}
		
		public override void After( MethodInfo methodUnderTest )
		{
			base.After( methodUnderTest );

			ServiceLocation.Assign( null );
		}
	}
}