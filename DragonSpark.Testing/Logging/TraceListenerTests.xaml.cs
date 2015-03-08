using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Application.Logging;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
			Logger.Log(  message, Category.Debug, Microsoft.Practices.Prism.Logging.Priority.None );

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
			Logger.Log( message, Category.Debug, Microsoft.Practices.Prism.Logging.Priority.None );

			Verify( message, Subject );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureInformationWritesAsExpected()
		{
			Listeners.Apply( x => Assert.AreEqual( 0, x.Output.Count ) );

			const string message = "This is an informational message";
			Runtime.Logging.Information( message );

			Verify( message, Information );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureWarningWritesAsExpected()
		{
			Listeners.Apply( x => Assert.AreEqual( 0, x.Output.Count ) );

			const string message = "This is a warning message";
			Runtime.Logging.Warning( message );

			Verify( message, Warning );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureErrorWritesAsExpected()
		{
			Listeners.Apply( x => Assert.AreEqual( 0, x.Output.Count ) );

			var exception = new ApplicationException( "An exception has occured." );
			Runtime.Logging.Error( exception );

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
