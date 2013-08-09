using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.Logging
{
	/// <summary>
	/// Interaction logic for TraceListenerTests.xaml
	/// </summary>
	[TestClass]
	public partial class TraceListenerTests
	{
		public TraceListenerTests()
		{
			typeof(LoggingExceptionHandler).ToString();
			InitializeComponent();
		}

		protected override void OnTestInitialized( EventArgs args )
		{
			base.OnTestInitialized( args );
			Listeners.Apply( x => x.Reset() );
		}
		
		[Dependency]
		public LogWriter LogWriter { get; set; }

		[Dependency( "Information" )]
		public TraceListener Information { get; set; }

		[Dependency( "Warning" )]
		public TraceListener Warning { get; set; }

		[Dependency( "Error" )]
		public TraceListener Error { get; set; }

		[Dependency]
		public IExceptionFormatter Formatter { get; set; }

		IEnumerable<TraceListener> Listeners
		{
			get { return new[] { Subject, Information, Warning, Error }; }
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureLogExists()
		{
			Assert.IsNotNull( Subject );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureLogWritesCorrectly()
		{
			Assert.IsFalse( Subject.Output.Any() );
			const string message = "This is a test.";
			DragonSpark.Logging.Log.Information( message, "Debug" );

			Assert.AreEqual( 1, Subject.Output.Count() );

			var logEntry = Subject.Log.Last();
			Assert.IsNotNull( logEntry );
			Assert.AreEqual( message, logEntry.Message );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureLogIsEmpty()
		{
			Assert.IsNotNull( Subject );
			Assert.IsFalse( Subject.Output.Any() );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureDebugWritesAsExpected()
		{
			Listeners.Apply( x => Assert.AreEqual( 0, x.Output.Count ) );

			const string message = "This is a debug message";
			DragonSpark.Logging.Log.Information( message, "Debug" );

			Verify( message, Subject );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureInformationWritesAsExpected()
		{
			Listeners.Apply( x => Assert.AreEqual( 0, x.Output.Count ) );

			const string message = "This is an informational message";
			DragonSpark.Logging.Log.Information( message );

			Verify( message, Information );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureWarningWritesAsExpected()
		{
			Listeners.Apply( x => Assert.AreEqual( 0, x.Output.Count ) );

			const string message = "This is a warning message";
			DragonSpark.Logging.Log.Warning( message );

			Verify( message, Warning );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureErrorWritesAsExpected()
		{
			Listeners.Apply( x => Assert.AreEqual( 0, x.Output.Count ) );

			var exception = new ApplicationException( "An exception has occured." );
			DragonSpark.Logging.Log.Error( exception );

			var message = Formatter.FormatMessage( exception );
			Verify( message, Error );
		}

		void Verify( string message, params TraceListener[] affected )
		{
			affected.Apply( x =>
			{
				Assert.AreEqual( message, x.Log.Last().Message );
				StringAssert.Contains( x.Output.Single(), message );
			} );
			Listeners.Except( affected ).Apply( x => Assert.IsFalse( x.Output.Any() ) );
		}

		protected override void OnTestCleanUp(EventArgs args)
		{
			LogWriter.Dispose();
			base.OnTestCleanUp(args);
		}
	}
}
