using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.Tracing;
using System.Linq;
using IExceptionHandler = DragonSpark.Exceptions.IExceptionHandler;

namespace DragonSpark.Testing.Runtime
{
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
			Registry.DisableAll();
			Registry.GetAll().OfType<TestEventListener>().Apply( x => x.Items.Clear() );
			Registry.EnableAll();
			// this.GetAllPropertyValuesOf<TraceListener>().Apply( x => x.Reset() );
		}

		[Dependency]
		public IEventListenerRegistry Registry { get; set; }

		[Dependency]
		public IExceptionHandler Handler { get; set; }

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void Exception()
		{
			Registry.EnableAll();

			var listeners = Registry.GetAll().OfType<TestEventListener>().ToArray();
			Assert.IsTrue( listeners.All( x => !x.Items.Any() ) );

			const string message = "This is an invalid operation.";
			
			var result = Handler.Handle( new InvalidOperationException( message ) );

			Assert.IsTrue( result.RethrowRecommended );

			Assert.IsInstanceOfType( result.Exception, typeof(ApplicationException) );
			
			var listener = listeners.FirstOrDefault( x => x.Items.Any() );
			Assert.IsNotNull( listener );
			Assert.AreEqual( EventLevel.Error, listener.Level );
			
			var args = listener.Items.Single();
			StringAssert.Contains( args.Payload.OfType<string>().First(), message );
			
			Assert.IsFalse( result.Exception.Message.Contains( "{handlingInstanceID}" ) );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void SpecificException()
		{
			Registry.EnableAll();

			var listeners = Registry.GetAll().OfType<TestEventListener>().ToArray();
			Assert.IsTrue( listeners.All( x => !x.Items.Any() ) );

			const string message = "This is a sepcific operation.";
			
			var result = Handler.Handle( new TestingException( message ) );

			Assert.IsFalse( result.RethrowRecommended );

			Assert.IsInstanceOfType( result.Exception, typeof(TestingException) );
			
			var listener = listeners.SingleOrDefault( x => x.Items.Any() );
			Assert.IsNotNull( listener );
			Assert.AreEqual( EventLevel.Error, listener.Level );
			
			var args = listener.Items.Single();
			StringAssert.Contains( args.Payload.OfType<string>().First(), message );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void FatalException()
		{
			Registry.EnableAll();

			var listeners = Registry.GetAll().OfType<TestEventListener>().ToArray();
			Assert.IsTrue( listeners.All( x => !x.Items.Any() ) );

			const string message = "This is a fatal operation.";
			
			var result = Handler.Handle( new FatalTestingException( message ) );

			Assert.IsTrue( result.RethrowRecommended );

			Assert.IsInstanceOfType( result.Exception, typeof(FatalTestingException) );
			
			var listener = listeners.SingleOrDefault( x => x.Items.Any() );
			Assert.IsNotNull( listener );
			Assert.AreEqual( EventLevel.Critical, listener.Level );
			
			var args = listener.Items.Single();
			StringAssert.Contains( args.Payload.OfType<string>().First(), message );
		}
	}
}
