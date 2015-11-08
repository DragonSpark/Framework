using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.Tracing;
using System.Linq;

namespace DragonSpark.Testing.Runtime
{
	[TestClass]
	public partial class Logging
	{
		public Logging()
		{
			InitializeComponent();
		}

		protected override void OnTestInitialized( EventArgs args )
		{
			base.OnTestInitialized( args );
			Registry.DisableAll();
			Registry.GetAll().OfType<TestEventListener>().Apply( x => x.Items.Clear() );
			Registry.EnableAll();
			// this.GetAllPropertyValuesOf<TraceListener>().Apply( x => x.Reset() );
		}

		[Dependency]
		public IEventListenerRegistry Registry { get; set; }

		[Dependency]
		public ITracer Tracer { get; set; }

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void Informational()
		{
			Registry.EnableAll();

			var listeners = Registry.GetAll().OfType<TestEventListener>().ToArray();
			Assert.IsTrue( listeners.All( x => !x.Items.Any() ) );

			const string message = "This is a test.";
			Log.Information( message );

			var listener = listeners.FirstOrDefault( x => x.Items.Any() );
			Assert.IsNotNull( listener );
			Assert.AreEqual( EventLevel.Informational, listener.Level );
			
			var args = listener.Items.Single();
			Assert.AreEqual( message, args.Payload.First() );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void Warning()
		{
			var listeners = Registry.GetAll().OfType<TestEventListener>().ToArray();
			Assert.IsTrue( listeners.All( x => !x.Items.Any() ) );

			const string message = "This is a warning.";
			Log.Warning( message );

			var listener = listeners.FirstOrDefault( x => x.Items.Any() );
			Assert.IsNotNull( listener );
			Assert.AreEqual( EventLevel.Warning, listener.Level );
			
			var args = listener.Items.Single();
			Assert.AreEqual( message, args.Payload.First() );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void Error()
		{
			var listeners = Registry.GetAll().OfType<TestEventListener>().ToArray();
			Assert.IsTrue( listeners.All( x => !x.Items.Any() ) );

			const string message = "This is an error.";
			Log.Error( new TestingException( message ) );

			var listener = listeners.FirstOrDefault( x => x.Items.Any() );
			Assert.IsNotNull( listener );
			Assert.AreEqual( EventLevel.Error, listener.Level );
			
			var args = listener.Items.Single();
			StringAssert.Contains( args.Payload.OfType<string>().First(), message );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void Critical()
		{
			var listeners = Registry.GetAll().OfType<TestEventListener>().ToArray();
			Assert.IsTrue( listeners.All( x => !x.Items.Any() ) );

			const string message = "This is a fatal error.";
			Log.Fatal( new FatalTestingException( message ) );

			var listener = listeners.FirstOrDefault( x => x.Items.Any() );
			Assert.IsNotNull( listener );
			Assert.AreEqual( EventLevel.Critical, listener.Level );
			
			var args = listener.Items.Single();
			StringAssert.Contains( args.Payload.OfType<string>().First(), message );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void Tracing()
		{
			var listeners = Registry.GetAll().OfType<TestEventListener>().ToArray();
			Assert.IsTrue( listeners.All( x => !x.Items.Any() ) );

			const string message = "This is a trace.";
			Log.Trace( message, () =>
			{
				Console.WriteLine( "Hello World" );
			} );

			var listener = listeners.FirstOrDefault( x => x.Items.Count == 2 );
			Assert.IsNotNull( listener );
			Assert.AreEqual( EventLevel.Informational, listener.Level );
			
			var first = listener.Items.First();
			Assert.AreEqual( first.Payload.OfType<string>().First(), message );

			var last = listener.Items.Last();
			Assert.AreNotEqual( last.Payload.OfType<string>().Last().Transform( TimeSpan.Parse ), TimeSpan.MinValue );
		}
	}
}
