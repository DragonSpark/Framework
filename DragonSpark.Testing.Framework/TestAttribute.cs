using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using Ploeh.AutoFixture;
using System.Linq;
using System.Reflection;
using DragonSpark.Testing.Framework.Extensions;
using Xunit.Sdk;

namespace DragonSpark.Testing.Framework
{
	public class TestAttribute : BeforeAfterTestAttribute
	{
		public override void After( MethodInfo methodUnderTest )
		{
			base.After( methodUnderTest );

			AmbientValues.Get<IFixture>( methodUnderTest ).With( fixture =>
			{
				fixture.Items().OfType<IAfterTestAware>().Each( aware => aware.After( fixture, methodUnderTest ) );
				AmbientValues.Remove( fixture );
			} );

			AmbientValues.Remove( methodUnderTest );
		}
	}
}