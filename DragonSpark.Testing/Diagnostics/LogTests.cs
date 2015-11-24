using DragonSpark.Diagnostics;
using DragonSpark.Testing.Framework;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using DragonSpark.Activation;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{

	// [Freeze( typeof(ApplicationInformation) )]
	public class LogTests
	{
		[Theory, Test, SetupAutoData]
		public void Information( [Frozen]Mock<ILogger> logger, string message )
		{
			logger.Object.Information( message );
			logger.Object.Information( message, Priority.High );

			logger.Verify( x => x.Information( message, Priority.Normal ) );
			logger.Verify( x => x.Information( message, Priority.High ) );
		}

		[Theory, Test, SetupAutoData]
		public void Warning( [Frozen]Mock<ILogger> logger, string message )
		{
			logger.Object.Warning( message );
			logger.Object.Warning( message, Priority.Low );

			logger.Verify( x => x.Warning( message, Priority.High ) );
			logger.Verify( x => x.Warning( message, Priority.Low ) );
		}

		[Theory, Test, SetupAutoData]
		public void Error( [Frozen]Mock<ILogger> logger, [Modest]InvalidOperationException error, string message )
		{
			// Assert.Same( logger, Log.Current );

			logger.Object.Exception( message, error );

			logger.Verify( x => x.Exception( message, error ) );
		}

		[Theory, Test, SetupAutoData]
		public void DefaultError( [Frozen]Mock<ILogger> logger, [Modest]InvalidOperationException error, string message )
		{
			logger.Object.Exception( message, error );

			// var message = error.ToString();

			logger.Verify( x => x.Exception( message, error ) );
		}

		[Theory, Test, SetupAutoData]
		public void Fatal( [Frozen]Mock<ILogger> logger, [Modest]InvalidOperationException error, string message )
		{
			// Assert.Same( logger, Log.Current );

			logger.Object.Fatal( message, error );

			// var message = formatter.FormatMessage( error, id );

			logger.Verify( x => x.Fatal( message, error ) );
		}

		[Theory, Test, SetupAutoData]
		public void Try( [Frozen, Registered]Mock<ILogger> logger )
		{
			var exception = DiagnosticExtensions.Try( () => {} );
			Assert.Null( exception );

			logger.VerifyAll();
		}

		[Theory, Test, SetupAutoData]
		public void TryException( [Frozen, Registered]Mock<ILogger> logger, [Modest]InvalidOperationException error )
		{
			var temp = Services.Location;
			var exception = DiagnosticExtensions.Try( () => { throw error; } );
			Assert.NotNull( exception );
			Assert.Equal( error, exception );

			// var message = formatter.FormatMessage( error );

			logger.Verify( x => x.Exception( "An exception has occurred while executing an application delegate.", exception ) );
		}

		/*[Theory, Test, Framework.AutoData, Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryAndHandleWithThrow( [Frozen]ILogger logger, [Frozen]IExceptionHandler handler, InvalidOperationException error, AggregateException thrown )
		{
			Mock.Get( handler ).Setup( x => x.Handle( error ) ).Returns( () => new ExceptionHandlingResult( true, thrown ) );

			Assert.Throws<AggregateException>( () => DiagnosticExtensions.TryAndHandle( () => { throw error; } ) );

			Mock.Get( logger ).Verify( x => x.Exception( It.IsAny<string>(), error ) );
		}

		[Theory, Test, Framework.AutoData, Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryAndHandle( [Frozen]ILogger logger, [Frozen]IExceptionHandler handler, InvalidOperationException error, AggregateException thrown )
		{
			var mock = Mock.Get( handler );
			mock.Setup( x => x.Handle( error ) ).Returns( () => new ExceptionHandlingResult( false, thrown ) );

			DiagnosticExtensions.TryAndHandle( () => { throw error; } );

			mock.VerifyAll();
			Mock.Get( logger ).Verify( x => x.Exception( It.IsAny<string>(), error ) );
		}*/
	}
}
