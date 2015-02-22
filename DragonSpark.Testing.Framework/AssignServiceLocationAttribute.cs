using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Activation;
using Xunit;

namespace DragonSpark.Testing.Framework
{
	[Priority( Priority.High )]
	public class AssignServiceLocationAttribute : BeforeAfterTestAttribute, ICustomization
	{
		static readonly List<string>  TestsList = new List<string>();

		public void Customize( IFixture fixture )
		{
			var locator = new ServiceLocator( fixture );
			ServiceLocation.Assign( locator );
		}

		public override void After( MethodInfo methodUnderTest )
		{
			TestsList.Add( methodUnderTest.Name );

			base.After( methodUnderTest );

			ServiceLocation.Assign( null );
		}
	}
}