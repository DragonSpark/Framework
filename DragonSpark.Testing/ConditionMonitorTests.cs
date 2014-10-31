using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using DragonSpark.Extensions;
using Xunit;
using Xunit.Extensions;
using Xunit.Sdk;

namespace DragonSpark.Testing
{
	/*public class TheoryAttribute : Xunit.Extensions.TheoryAttribute
	{
		protected override IEnumerable<ITestCommand> EnumerateTestCommands( IMethodInfo method )
		{
			return base.EnumerateTestCommands( method ).Concat( this.AsItem() );
		}
	}*/

	public class ConditionMonitorTests
	{
		[Theory, Framework.AutoMockData]
		public void Apply( ConditionMonitor sut )
		{
			Assert.False( sut.Applied );

			var applied = sut.Apply();

			Assert.True( applied );
			Assert.True( sut.Applied );


			Assert.False( sut.Apply() );
			
			sut.Reset();

			Assert.False( sut.Applied );
		}

		[Theory, Framework.AutoMockData]
		public void ApplyWithAction( ConditionMonitor sut )
		{
			var count = 0;

			Assert.True( sut.Apply( () => count++ ) );

			Assert.Equal( 1, count );

			Assert.False( sut.Apply( () => count++ ) );

			Assert.Equal( 1, count );
		}

		[Theory, Framework.AutoMockData]
		public void ApplyIf( ConditionMonitor sut )
		{
			var count = 0;

			Assert.False( sut.ApplyIf( () => false, () => count++ ) );

			Assert.Equal( 0, count );

			Assert.True( sut.ApplyIf( () => true, () => count++ ) );

			Assert.Equal( 1, count );

			Assert.False( sut.ApplyIf( () => true, () => count++ ) );
		}
	}
}