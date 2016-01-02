using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Extensions;
using DragonSpark.Testing.Framework.Setup;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace DragonSpark.Testing.Framework
{
	public class TestAttribute : BeforeAfterTestAttribute
	{
		readonly CurrentExecution execution;

		public TestAttribute() : this( CurrentExecution.Instance )
		{}

		public TestAttribute( CurrentExecution execution )
		{
			this.execution = execution;
		}

		public override void Before( MethodInfo methodUnderTest )
		{
			using ( new ExecutionContext( execution, methodUnderTest ) )
			{
				base.Before( methodUnderTest );

				new AssociatedFixture( methodUnderTest ).Item.With( fixture =>
				{
					fixture.Items().OfType<ITestExecutionAware>().Each( aware => aware.Before( fixture, methodUnderTest ) );
				} );
			}
		}

		public override void After( MethodInfo methodUnderTest )
		{
			using ( new ExecutionContext( execution, methodUnderTest ) )
			{
				base.After( methodUnderTest );

				new AssociatedFixture( methodUnderTest ).Item.With( fixture =>
				{
					fixture.Items().OfType<ITestExecutionAware>().Each( aware => aware.After( fixture, methodUnderTest ) );
				} );
			}
		}
	}
}