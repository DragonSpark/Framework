﻿using DragonSpark.Diagnostics;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class LogTests
	{
		[Theory, Test, SetupAutoData]
		public void Information( [Located(false), Frozen]IMessageLogger messageLogger, string message )
		{
			messageLogger.Information( message );
			messageLogger.Information( message, Priority.High );

			Mock.Get( messageLogger ).Verify( x => x.Information( message, Priority.Normal ) );
			Mock.Get( messageLogger ).Verify( x => x.Information( message, Priority.High ) );
		}

		[Theory, Test, SetupAutoData]
		public void Warning( [Located(false), Frozen]IMessageLogger messageLogger, string message )
		{
			messageLogger.Warning( message );
			messageLogger.Warning( message, Priority.Low );

			Mock.Get( messageLogger ).Verify( x => x.Warning( message, Priority.High ) );
			Mock.Get( messageLogger ).Verify( x => x.Warning( message, Priority.Low ) );
		}

		[Theory, Test, SetupAutoData]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Error( [Located(false), Frozen]IMessageLogger messageLogger, IExceptionFormatter formatter, [Modest]InvalidOperationException error, string message )
		{
			// Assert.Same( logger, Log.Current );

			messageLogger.Exception( message, error );

			Mock.Get( messageLogger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, Test, SetupAutoData]
		public void DefaultError( [Located(false), Frozen]IMessageLogger messageLogger, [Modest]InvalidOperationException error, string message )
		{
			messageLogger.Exception( message, error );

			// var message = error.ToString();

			Mock.Get( messageLogger ).Verify( x => x.Exception( message, error ) );
		}

		[Theory, Test, SetupAutoData]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Fatal( [Located(false), Frozen]IMessageLogger messageLogger, IExceptionFormatter formatter, [Modest]InvalidOperationException error, string message )
		{
			// Assert.Same( logger, Log.Current );

			messageLogger.Fatal( message, error );

			// var message = formatter.FormatMessage( error, id );

			Mock.Get( messageLogger ).Verify( x => x.Fatal( message, error ) );
		}

		[Theory, Test, SetupAutoData]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void Try()
		{
			var exception = DiagnosticExtensions.Try( () => {} );
			Assert.Null( exception );
		}

		[Theory, Test, SetupAutoData]
		[Register( typeof(IExceptionFormatter), typeof(ExceptionFormatter) )]
		public void TryException( [Located(false), Frozen, Registered]IMessageLogger messageLogger, [Modest]InvalidOperationException error )
		{
			var exception = DiagnosticExtensions.Try( () => { throw error; } );
			Assert.NotNull( exception );
			Assert.Equal( error, exception );

			// var message = formatter.FormatMessage( error );

			Mock.Get( messageLogger ).Verify( x => x.Exception( "An exception has occurred while executing an application delegate.", exception ) );
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
