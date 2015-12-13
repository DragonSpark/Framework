using DragonSpark.Extensions;
using System;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class ExceptionExtensionsTests
	{
		[Fact]
		public void CanRegisterFrameworkExceptionTypes()
		{
			Assert.False( ExceptionExtensions.IsFrameworkExceptionRegistered( typeof(MockException) ) );

			ExceptionExtensions.RegisterFrameworkExceptionType( typeof(MockException) );

			Assert.True( ExceptionExtensions.IsFrameworkExceptionRegistered( typeof(MockException) ) );
		}

		[Fact]
		public void CanGetRootException()
		{
			Exception caughtException;
			ExceptionExtensions.RegisterFrameworkExceptionType( typeof(FrameworkException1) );
			try
			{
				try
				{
					throw new RootException();
				}
				catch ( Exception ex )
				{
					throw new FrameworkException1( ex );
				}
			}
			catch ( Exception ex )
			{
				caughtException = ex;
			}

			Assert.NotNull( caughtException );

			var exception = caughtException.GetRootException();

			Assert.IsType<RootException>( exception );
		}

		[Fact]
		public void CanCompensateForInnerFrameworkExceptionType()
		{
			Exception caughtException;
			ExceptionExtensions.RegisterFrameworkExceptionType( typeof(FrameworkException2) );
			try
			{
				try
				{
					try
					{
						throw new RootException();
					}
					catch ( Exception ex )
					{
						throw new FrameworkException2( ex );
					}
				}
				catch ( Exception ex )
				{
					throw new NonFrameworkException( ex );
				}
			}
			catch ( Exception ex )
			{
				caughtException = ex;
			}

			Assert.NotNull( caughtException );

			var exception = caughtException.GetRootException();
			Assert.IsType<RootException>( exception );
		}

		[Fact]
		public void GetRootExceptionReturnsTopExceptionWhenNoUserExceptionFound()
		{
			Exception caughtException;
			ExceptionExtensions.RegisterFrameworkExceptionType( typeof(FrameworkException1) );
			ExceptionExtensions.RegisterFrameworkExceptionType( typeof(FrameworkException2) );
			try
			{
				try
				{
					throw new FrameworkException1( null );
				}
				catch ( Exception ex )
				{
					throw new FrameworkException2( ex );
				}
			}
			catch ( Exception ex )
			{
				caughtException = ex;
			}

			Assert.NotNull( caughtException );

			var exception = caughtException.GetRootException();
			Assert.IsType<FrameworkException2>( exception );
		}

		class MockException : Exception
		{}

		private class FrameworkException2 : Exception
		{
			public FrameworkException2( Exception innerException ) : base( string.Empty, innerException )
			{
			}
		}

		private class FrameworkException1 : Exception
		{
			public FrameworkException1( Exception innerException ) : base( string.Empty, innerException )
			{
			}
		}

		class RootException : Exception
		{}

		class NonFrameworkException : Exception
		{
			public NonFrameworkException( Exception innerException ) : base( string.Empty, innerException )
			{
			}
		}
	}
}