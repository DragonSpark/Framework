namespace DragonSpark.Testing.Diagnostics
{

	/*[Freeze( typeof(ApplicationInformation) )]
	public class LogTests
	{
		[Theory, AutoMockData, Services]
		public void Information( [Frozen]ILogger logger, string message )
		{
			Log.Current.Information( message );
			Log.Current.Information( message, Priority.High );

			Mock.Get( logger ).Verify( x => x.Information( message, Priority.Normal ) );
			Mock.Get( logger ).Verify( x => x.Information( message, Priority.High ) );
		}

		[Theory, AutoMockData, Services]
		public void Warning( [Frozen]ILogger logger, string message )
		{
			Log.Warning( message );
			Log.Warning( message, Priority.Low );

			Mock.Get( logger ).Verify( x => x.Warning( message, Priority.High ) );
			Mock.Get( logger ).Verify( x => x.Warning( message, Priority.Low ) );
		}

		[Theory, AutoMockData, Services]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Error( [Frozen]ILogger logger, IExceptionFormatter formatter, InvalidOperationException error, Guid id )
		{
			Assert.Same( logger, Log.Current );

			Log.Current.Error( error, id );

			var message = formatter.FormatMessage( error, id );

			Mock.Get( logger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, AutoMockData, Services]
		public void DefaultError( [Frozen]ILogger logger, InvalidOperationException error, Guid id )
		{
			Assert.Same( logger, Log.Current );

			Log.Current.Error( error, id );

			var message = error.ToString();

			Mock.Get( logger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, AutoMockData, Services]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Fatal( [Frozen]ILogger logger, IExceptionFormatter formatter, InvalidOperationException error, Guid id )
		{
			Assert.Same( logger, Log.Current );

			Log.Current.Fatal( error, id );

			var message = formatter.FormatMessage( error, id );

			Mock.Get( logger ).Verify( x => x.Fatal( message, error ) );
		}

		[Theory, AutoDataCustomization, Services]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Try()
		{
			var exception = DiagnosticExtensions.Try( () => {} );
			Assert.Null( exception );
		}

		[Theory, AutoMockData, Services]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryException( [Frozen]ILogger logger, IExceptionFormatter formatter, InvalidOperationException error )
		{
			var exception = DiagnosticExtensions.Try( () => { throw error; } );
			Assert.NotNull( exception );
			Assert.Equal( error, exception );

			var message = formatter.FormatMessage( error );

			Mock.Get( logger ).Verify( x => x.Exception( message, exception ) );
		}

		[Theory, AutoMockData, Services, Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryAndHandleWithThrow( [Frozen]ILogger logger, [Frozen]IExceptionHandler handler, InvalidOperationException error, AggregateException thrown )
		{
			Mock.Get( handler ).Setup( x => x.Handle( error ) ).Returns( () => new ExceptionHandlingResult( true, thrown ) );

			Assert.Throws<AggregateException>( () => DiagnosticExtensions.TryAndHandle( () => { throw error; } ) );

			Mock.Get( logger ).Verify( x => x.Exception( It.IsAny<string>(), error ) );
		}

		[Theory, AutoMockData, Services, Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryAndHandle( [Frozen]ILogger logger, [Frozen]IExceptionHandler handler, InvalidOperationException error, AggregateException thrown )
		{
			var mock = Mock.Get( handler );
			mock.Setup( x => x.Handle( error ) ).Returns( () => new ExceptionHandlingResult( false, thrown ) );

			DiagnosticExtensions.TryAndHandle( () => { throw error; } );

			mock.VerifyAll();
			Mock.Get( logger ).Verify( x => x.Exception( It.IsAny<string>(), error ) );
		}
	}*/
}
