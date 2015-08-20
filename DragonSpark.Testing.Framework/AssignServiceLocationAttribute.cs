using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Activation.IoC;
using Microsoft.Practices.ServiceLocation;
using Xunit.Sdk;

namespace DragonSpark.Testing.Framework
{
	[Priority( Priority.High )]
	public class AssignServiceLocationAttribute : BeforeAfterTestAttribute, ICustomization
	{
		static readonly List<string> TestsList = new List<string>();

		public void Customize( IFixture fixture )
		{
			var locator = CreateLocator( fixture );
			Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider( () => locator );
		}

		protected virtual IServiceLocator CreateLocator( IFixture fixture )
		{
			return new ServiceLocator( fixture );
		}

		public override void After( MethodInfo methodUnderTest )
		{
			TestsList.Add( methodUnderTest.Name );

			base.After( methodUnderTest );

			Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider( null );
		}
	}

	/*public class CompositeServiceLocationAttribute : AssignServiceLocationAttribute
	{
		protected override IServiceLocator CreateLocator( IFixture fixture )
		{
			var result = new CompositeServiceLocator( base.CreateLocator( fixture ), new Activation.IoC.ServiceLocator() );
			return result;
		}
	}*/
}