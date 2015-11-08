using System;
using System.Linq;
using DragonSpark.Testing.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Logging
{
	/// <summary>
	/// Interaction logic for TraceListenerWithTracingTests.xaml
	/// </summary>
	[TestClass]
	public partial class TraceListenerWithTracingTests
	{
		public TraceListenerWithTracingTests()
		{
			InitializeComponent();
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond ), Subject( "Tracer" )]
		public void EnsureTraceWorksCorrectly()
		{
			Assert.AreEqual( 0, Subject.Output.Count() );
			DragonSpark.Logging.Log.Trace( () => Console.Write( @"Hello World." ) );
			Assert.AreEqual( 2, Subject.Output.Count() );
		}
	}
}
