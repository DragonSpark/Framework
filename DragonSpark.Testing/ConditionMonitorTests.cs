using Xunit;

namespace DragonSpark.Testing
{
	public class ConditionMonitorTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Apply( ConditionMonitor sut )
		{
			Assert.False( sut.State == ConditionMonitorState.Applied );

			var applied = sut.Apply();

			Assert.True( applied );
			Assert.True( sut.State == ConditionMonitorState.Applied );


			Assert.False( sut.Apply() );
			
			sut.Reset();

			Assert.False( sut.State == ConditionMonitorState.Applied );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void ApplyWithAction( ConditionMonitor sut )
		{
			var count = 0;

			Assert.True( sut.Apply( () => count++ ) );

			Assert.Equal( 1, count );

			Assert.False( sut.Apply( () => count++ ) );

			Assert.Equal( 1, count );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void ApplyIf( ConditionMonitor sut )
		{
			var count = 0;

			Assert.False( sut.ApplyIf( () => false, () => count++ ) );
			
			Assert.Equal( 0, count );

			Assert.True( sut.ApplyIf( () => true, () => count++ ) );

			Assert.Equal( 1, count );

			Assert.False( sut.ApplyIf( () => true, () => count++ ) );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void ApplyIfNull( ConditionMonitor sut )
		{
			var count = 0;

			Assert.True( sut.ApplyIf( null, () => count++ ) );

			Assert.Equal( 1, count );
		}
	}
}