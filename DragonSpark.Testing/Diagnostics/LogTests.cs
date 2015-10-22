using DragonSpark.Diagnostics;
using Ploeh.AutoFixture.Xunit2;
using ExceptionFormatter = DragonSpark.Windows.Runtime.ExceptionFormatter;

namespace DragonSpark.Testing.Diagnostics
{
	using Framework;
	using Moq;
	using System;
	using Xunit;

	[Freeze( typeof(ApplicationInformation) )]
	public class LogTests
	{
		/*[Theory, AutoMockData, AssignServiceLocation]
		public void Information( [Frozen]ILogger logger, string message )
		{
			Log.Information( message );
			Log.Information( message, Priority.High );

			Mock.Get( logger ).Verify( x => x.Information( message, Priority.Normal ) );
			Mock.Get( logger ).Verify( x => x.Information( message, Priority.High ) );
		}

		[Theory, AutoMockData, AssignServiceLocation]
		public void Warning( [Frozen]ILogger logger, string message )
		{
			Log.Warning( message );
			Log.Warning( message, Priority.Low );

			Mock.Get( logger ).Verify( x => x.Warning( message, Priority.High ) );
			Mock.Get( logger ).Verify( x => x.Warning( message, Priority.Low ) );
		}*/

		[Theory, AutoMockData, AssignServiceLocation]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Error( [Frozen]ILogger logger, IExceptionFormatter formatter, InvalidOperationException error, Guid id )
		{
			Log.Error( error, id );

			var message = formatter.FormatMessage( error, id );

			Mock.Get( logger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, AutoMockData, AssignServiceLocation]
		public void DefaultError( [Frozen]ILogger logger, InvalidOperationException error, Guid id )
		{
			Log.Error( error, id );

			var message = error.ToString();

			Mock.Get( logger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, AutoMockData, AssignServiceLocation]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Fatal( [Frozen]ILogger logger, IExceptionFormatter formatter, InvalidOperationException error, Guid id )
		{
			Log.Fatal( error, id );

			var message = formatter.FormatMessage( error, id );

			Mock.Get( logger ).Verify( x => x.Fatal( message, error ) );
		}

		/*[Theory, AutoMockData, AssignServiceLocation]
		[Freeze( typeof(ITracer), typeof(Tracer) )]
		public void Trace( [Frozen]ILogger logger, string message )
		{
			var called = false;
			Log.Trace( () => called = true, message );
			Assert.True( called );
			Mock.Get( logger ).Verify( x => x.StartTrace( message, It.IsAny<Guid>() ) );
			Mock.Get( logger ).Verify( x => x.EndTrace( message, It.IsAny<Guid>(), It.Is<TimeSpan>( y => y > TimeSpan.Zero ) ) );
		}

		[Theory, AutoMockData, AssignServiceLocation]
		[Freeze( typeof(ITracer), typeof(Tracer) )]
		public void TraceWithId( [Frozen]ILogger logger, string message, Guid id )
		{
			var called = false;
			Log.Trace( () => called = true, message, id );
			Assert.True( called );

			Assert.Equal( Services.Locate<ITracer>(), Services.Locate<ITracer>() );
			Assert.Equal( Services.Locate<ITracer>(), Services.Locate<Tracer>() );
			Mock.Get( logger ).Verify( x => x.StartTrace( message, id ) );
			Mock.Get( logger ).Verify( x => x.EndTrace( message, id, It.Is<TimeSpan>( y => y > TimeSpan.Zero ) ) );
		}*/

		[Theory, AutoDataCustomization, AssignServiceLocation]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Try()
		{
			var exception = Log.Try( () => {} );
			Assert.Null( exception );
		}

		[Theory, AutoMockData, AssignServiceLocation]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryException( [Frozen]ILogger logger, IExceptionFormatter formatter, InvalidOperationException error )
		{
			var exception = Log.Try( () => { throw error; } );
			Assert.NotNull( exception );
			Assert.Equal( error, exception );

			var message = formatter.FormatMessage( error );

			Mock.Get( logger ).Verify( x => x.Exception( message, exception ) );
		}

		[Theory, AutoMockData, AssignServiceLocation, Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryAndHandleWithThrow( [Frozen]ILogger logger, [Frozen]IExceptionHandler handler, InvalidOperationException error, AggregateException thrown )
		{
			Mock.Get( handler ).Setup( x => x.Handle( error ) ).Returns( () => new ExceptionHandlingResult( true, thrown ) );

			Assert.Throws<AggregateException>( () => Log.TryAndHandle( () => { throw error; } ) );

			Mock.Get( logger ).Verify( x => x.Exception( It.IsAny<string>(), error ) );
		}

		[Theory, AutoMockData, AssignServiceLocation, Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryAndHandle( [Frozen]ILogger logger, [Frozen]IExceptionHandler handler, InvalidOperationException error, AggregateException thrown )
		{
			var mock = Mock.Get( handler );
			mock.Setup( x => x.Handle( error ) ).Returns( () => new ExceptionHandlingResult( false, thrown ) );

			Log.TryAndHandle( () => { throw error; } );

			mock.VerifyAll();
			Mock.Get( logger ).Verify( x => x.Exception( It.IsAny<string>(), error ) );
		}
	}
}
