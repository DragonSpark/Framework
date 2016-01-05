using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup;
using Moq;
using System;
using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class CompositeMessageLoggerTests
	{
		[Theory, AutoData]
		public void Information( IMessageLogger sut, IMessageLogger other, string message, Priority priority )
		{
			var composite = new CompositeMessageLogger( sut, other );
			composite.Information( message, priority );

			new[] { sut, other }.Each( x => Mock.Get( x ).Verify( logger => logger.Information( message, priority ) ) );
		}

		[Theory, AutoData]
		public void Warning( IMessageLogger sut, IMessageLogger other, string message, Priority priority )
		{
			var composite = new CompositeMessageLogger( sut, other );
			composite.Warning( message, priority );

			new[] { sut, other }.Each( x => Mock.Get( x ).Verify( logger => logger.Warning( message, priority ) ) );
		}

		[Theory, AutoData]
		public void Exception( IMessageLogger sut, IMessageLogger other, string message, Exception error )
		{
			var composite = new CompositeMessageLogger( sut, other );
			composite.Exception( message, error );

			new[] { sut, other }.Each( x => Mock.Get( x ).Verify( logger => logger.Exception( message, error ) ) );
		}

		[Theory, AutoData]
		public void Fatal( IMessageLogger sut, IMessageLogger other, string message, FatalApplicationException error )
		{
			var composite = new CompositeMessageLogger( sut, other );
			composite.Fatal( message, error );

			new[] { sut, other }.Each( x => Mock.Get( x ).Verify( logger => logger.Fatal( message, error ) ) );
		}

		[Theory, AutoDataMoq]
		public void Dispose( Mock<IMessageLogger> sut )
		{
			var disposable = sut.As<IDisposable>();

			using ( var composite = new CompositeMessageLogger( sut.Object ) )
			{
				composite.Information( "Hello", Priority.Normal );
			}

			disposable.Verify( d => d.Dispose() );
		}
	}
}