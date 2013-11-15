using DragonSpark.IoC;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.Practices.Unity;

namespace DragonSpark.Testing.IoC
{
	/// <summary>
	/// Interaction logic for UnityContainerExtensionsTests.xaml
	/// </summary>
	[TestClass]
	public partial class UnityContainerExtensionsTests
	{
		public UnityContainerExtensionsTests()
		{
			InitializeComponent();
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureResolveAllWorksAsExpected()
		{
			var defaultResolve = Subject.ResolveAll<INamedObject>();
			Assert.AreEqual( 2, defaultResolve.Count() );

			var items = Subject.ResolveAll<INamedObject>();
			Assert.AreEqual( 4, items.Count() );

			var first = items.First();
			Assert.AreEqual( first.Name, "Instance Registration" );

			var last = items.Last();
			Assert.AreEqual( last.Name, "Build2 Registration" );
		}
	}
}
