using DragonSpark.Diagnostics;
using DragonSpark.Testing.Framework;
using Moq;
using System;
using Xunit;
using Xunit.Extensions;
using ExceptionFormatter = DragonSpark.Testing.Server.ExceptionFormatter;

namespace DragonSpark.Testing.Diagnostics
{
	[Freeze( typeof(ApplicationDetails) )]
	public class LogTests
	{
		[Theory, AutoData, AssignServiceLocation]
		public void Information( [FreezeObject] Mock<ILogger> logger, string message )
		{
			Log.Information( message );
			Log.Information( message, Priority.High );

			logger.Verify( x => x.Information( message, Priority.Normal ) );
			logger.Verify( x => x.Information( message, Priority.High ) );
		}

		[Theory, AutoData, AssignServiceLocation]
		public void Warning( [FreezeObject] Mock<ILogger> logger, string message )
		{
			Log.Warning( message );
			Log.Warning( message, Priority.Low );

			logger.Verify( x => x.Warning( message, Priority.High ) );
			logger.Verify( x => x.Warning( message, Priority.Low ) );
		}

		[Theory, AutoData, AssignServiceLocation]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Error( [FreezeObject]Mock<ILogger> logger, IExceptionFormatter formatter, InvalidOperationException error, Guid id )
		{
			Log.Error( error, id );

			var message = formatter.FormatMessage( error, id );

			logger.Verify( x => x.Exception( message, error ) );
		}

		[Theory, AutoData, AssignServiceLocation]
		public void DefaultError( [FreezeObject]Mock<ILogger> logger, InvalidOperationException error, Guid id )
		{
			Log.Error( error, id );

			var message = error.ToString();

			logger.Verify( x => x.Exception( message, error ) );
		}

		[Theory, AutoData, AssignServiceLocation]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Fatal( [FreezeObject]Mock<ILogger> logger, IExceptionFormatter formatter, InvalidOperationException error, Guid id )
		{
			Log.Fatal( error, id );

			var message = formatter.FormatMessage( error, id );

			logger.Verify( x => x.Fatal( message, error ) );
		}

		[Theory, AutoData, AssignServiceLocation]
		[Freeze( typeof(ITracer), typeof(Tracer), After = true )]
		public void Trace( [FreezeObject] Mock<ILogger> logger, string message )
		{
			var called = false;
			Log.Trace( () => called = true, message );
			Assert.True( called );
			logger.Verify( x => x.StartTrace( message, It.IsAny<Guid>() ) );
			logger.Verify( x => x.EndTrace( message, It.IsAny<Guid>(), It.Is<TimeSpan>( y => y > TimeSpan.Zero ) ) );
		}

		[Theory, AutoData, AssignServiceLocation]
		[Freeze( typeof(ITracer), typeof(Tracer), After = true )]
		public void TraceWithId( [FreezeObject] Mock<ILogger> logger, string message, Guid id )
		{
			var called = false;
			Log.Trace( () => called = true, message, id );
			Assert.True( called );
			logger.Verify( x => x.StartTrace( message, id ) );
			logger.Verify( x => x.EndTrace( message, id, It.Is<TimeSpan>( y => y > TimeSpan.Zero ) ) );
		}

		[Theory, AutoData, AssignServiceLocation]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Try()
		{
			var exception = Log.Try( () => {} );
			Assert.Null( exception );
		}

		[Theory, AutoData, AssignServiceLocation]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryException( [FreezeObject]Mock<ILogger> logger, IExceptionFormatter formatter, InvalidOperationException error )
		{
			var exception = Log.Try( () => { throw error; } );
			Assert.NotNull( exception );
			Assert.Equal( error, exception );

			var message = formatter.FormatMessage( error );

			logger.Verify( x => x.Exception( message, exception ) );
		}

		[Theory, AutoData, AssignServiceLocation, Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryAndHandleWithThrow( [FreezeObject] Mock<ILogger> logger, [FreezeObject] Mock<IExceptionHandler> handler, InvalidOperationException error, AggregateException thrown )
		{
			handler.Setup( x => x.Handle( error ) ).Returns( () => new ExceptionHandlingResult( true, thrown ) );

			Assert.Throws<AggregateException>( () => Log.TryAndHandle( () => { throw error; } ) );

			logger.Verify( x => x.Exception( It.IsAny<string>(), error ) );
		}

		[Theory, AutoData, AssignServiceLocation, Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryAndHandle( [FreezeObject] Mock<ILogger> logger, [FreezeObject] Mock<IExceptionHandler> handler, InvalidOperationException error, AggregateException thrown )
		{
			handler.Setup( x => x.Handle( error ) ).Returns( () => new ExceptionHandlingResult( false, thrown ) );

			Log.TryAndHandle( () => { throw error; } );

			handler.VerifyAll();
			logger.Verify( x => x.Exception( It.IsAny<string>(), error ) );

		}
	}
}
