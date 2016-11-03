using DragonSpark.Aspects.Exceptions;
using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Diagnostics.Exceptions;
using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using Polly;
using System;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Aspects.Exceptions
{
	public class ApplyExceptionPolicyAttributeTests
	{
		[Fact]
		public void VerifyInstance()
		{
			var source = SuppliedRetryPolicySource<CustomException>.Default;
			Assert.Same( source.Get(), source.Get() );
		}

		[Fact]
		public void AppliedCommand()
		{
			var sut = new Command();
			var history = LoggingHistory.Default.Get();
			Assert.Empty( history.Events );
			Assert.Equal( 0, sut.Called );
			Assert.Throws<CustomException>( () => sut.Execute( true ) );
			Assert.Equal( 4, sut.Called );
			Assert.Equal( 3, history.Events.Count() );
		}

		sealed class RetrySource<T> : SuppliedRetryPolicySource<T> where T : Exception
		{
			[UsedImplicitly]
			public new static ISource<Policy> Default { get; } = new RetrySource<T>().ToSingletonScope();
			RetrySource() : base( new RetryPolicySource<T>( Time.None ), 3 ) {}
		}

		[ApplyExceptionPolicy( typeof(RetrySource<CustomException>) )]
		sealed class Command : CommandBase<bool>
		{
			public int Called { get; private set; }

			public override void Execute( bool parameter )
			{
				Called++;
				if ( parameter )
				{
					throw new CustomException();
				}
			}
		}

		class CustomException : Exception {}
	}
}