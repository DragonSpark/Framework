﻿using DragonSpark.Diagnostics;
using DragonSpark.Testing.Framework;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{

	// [Freeze( typeof(ApplicationInformation) )]
	public class LogTests
	{
		[Theory, Test, Framework.AutoData]
		public void Information( [Frozen, SkipLocation]ILogger logger, string message )
		{
			logger.Information( message );
			logger.Information( message, Priority.High );

			Mock.Get( logger ).Verify( x => x.Information( message, Priority.Normal ) );
			Mock.Get( logger ).Verify( x => x.Information( message, Priority.High ) );
		}

		[Theory, Test, Framework.AutoData]
		public void Warning( [Frozen, SkipLocation]ILogger logger, string message )
		{
			logger.Warning( message );
			logger.Warning( message, Priority.Low );

			Mock.Get( logger ).Verify( x => x.Warning( message, Priority.High ) );
			Mock.Get( logger ).Verify( x => x.Warning( message, Priority.Low ) );
		}

		[Theory, Test, Framework.AutoData]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Error( [Frozen, SkipLocation]ILogger logger, IExceptionFormatter formatter, [Modest]InvalidOperationException error, string message )
		{
			// Assert.Same( logger, Log.Current );

			logger.Exception( message, error );

			Mock.Get( logger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, Test, Framework.AutoData]
		public void DefaultError( [Frozen, SkipLocation]ILogger logger, [Modest]InvalidOperationException error, string message )
		{
			logger.Exception( message, error );

			// var message = error.ToString();

			Mock.Get( logger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, Test, Framework.AutoData]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Fatal( [Frozen, SkipLocation]ILogger logger, IExceptionFormatter formatter, [Modest]InvalidOperationException error, string message )
		{
			// Assert.Same( logger, Log.Current );

			logger.Fatal( message, error );

			// var message = formatter.FormatMessage( error, id );

			Mock.Get( logger ).Verify( x => x.Fatal( message, error ) );
		}

		[Theory, Test, Framework.AutoData]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Try()
		{
			var exception = DiagnosticExtensions.Try( () => {} );
			Assert.Null( exception );
		}

		[Theory, Test, Framework.AutoData]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryException( [Frozen, SkipLocation, Registered]ILogger logger, [Modest]InvalidOperationException error )
		{
			var exception = DiagnosticExtensions.Try( () => { throw error; } );
			Assert.NotNull( exception );
			Assert.Equal( error, exception );

			// var message = formatter.FormatMessage( error );

			Mock.Get( logger ).Verify( x => x.Exception( "An exception has occurred while executing an application delegate.", exception ) );
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
