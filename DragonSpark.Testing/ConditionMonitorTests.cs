using System.Runtime;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing
{
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