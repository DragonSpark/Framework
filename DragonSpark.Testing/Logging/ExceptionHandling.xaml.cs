using System;
using System.Linq;
using DragonSpark.Application.Logging;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Logging
{
	/// <summary>
	/// Interaction logic for ExceptionHandling.xaml
	/// </summary>
	[TestClass]
	public partial class ExceptionHandling
	{
		public ExceptionHandling()
		{
			InitializeComponent();
		}

		protected override void OnTestInitialized( EventArgs args )
		{
			base.OnTestInitialized( args );
			this.GetAllPropertyValuesOf<TraceListener>().Apply( x => x.Reset() );
		}
		
		[Dependency( "Error" )]
		public TraceListener Error { get; set; }

		[Dependency( "Critical" )]
		public TraceListener Critical { get; set; }

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureSubjectIsNotNull()
		{
			Assert.IsNotNull( Subject );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureExceptionIsHandledAsExpected()
		{
			this.GetAllPropertyValuesOf<TraceListener>().Apply( x => Assert.IsFalse( x.Output.Any() ) );

			var exception = new InvalidOperationException( "This is an invalid operation" );
			Exception thrown = null;
			try
			{
				Runtime.Logging.TryAndHandle(() => { throw exception; } );
			}
			catch ( Exception e )
			{
				thrown = e;
			}

			Assert.IsInstanceOfType( thrown, typeof(ApplicationException) );
			Assert.AreEqual( exception, thrown.InnerException );

			Assert.AreEqual( 1, Error.Output.Count );
			Assert.IsFalse( Critical.Output.Any() );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureTestingExceptionIsHandledAsExpected()
		{
			this.GetAllPropertyValuesOf<TraceListener>().Apply( x => Assert.IsFalse( x.Output.Any() ) );

			var exception = new TestingException( "This is an testing exception." );

			Runtime.Logging.TryAndHandle( () => { throw exception; } );


			Assert.AreEqual( 1, Error.Output.Count );
			Assert.IsFalse( Critical.Output.Any() );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureFatalTestingExceptionIsHandledAsExpected()
		{
			this.GetAllPropertyValuesOf<TraceListener>().Apply( x => Assert.IsFalse( x.Output.Any() ) );

			var exception = new FatalTestingException( "This is an fatal exception." );
			Exception thrown = null;
			try
			{
				Runtime.Logging.TryAndHandle( () => { throw exception; } );
			}
			catch ( Exception e )
			{
				thrown = e;
			}
			Assert.IsInstanceOfType( thrown, typeof(FatalTestingException) );
			Assert.AreEqual( exception, thrown );

			Assert.AreEqual( 1, Critical.Output.Count );
			Assert.IsFalse( Error.Output.Any() );
		}
	}
}
