using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Extensions;
using Ploeh.AutoFixture;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace DragonSpark.Testing.Framework
{
	public class TestAttribute : BeforeAfterTestAttribute
	{
		public override void Before( MethodInfo methodUnderTest )
		{
			base.Before( methodUnderTest );

			AmbientValues.Get<IFixture>( methodUnderTest ).With( fixture =>
			{
				fixture.Items().OfType<ITestExecutionAware>().Each( aware => aware.Before( fixture, methodUnderTest ) );
			} );
		}

		public override void After( MethodInfo methodUnderTest )
		{
			base.After( methodUnderTest );

			AmbientValues.Get<IFixture>( methodUnderTest ).With( fixture =>
			{
				fixture.Items().OfType<ITestExecutionAware>().Each( aware => aware.After( fixture, methodUnderTest ) );
				AmbientValues.Remove( fixture );
			} );

			AmbientValues.Remove( methodUnderTest );
		}
	}
}