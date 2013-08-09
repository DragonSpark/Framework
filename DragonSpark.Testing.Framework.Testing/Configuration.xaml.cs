using System;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Framework.Testing
{
	/// <summary>
	/// Interaction logic for Configuration.xaml
	/// </summary>
	[TestClass]
	public partial class Configuration
	{
		const string HelloWorld = "Hello World!";

		public Configuration()
		{
			InitializeComponent();
		}

		protected override void OnTestInitializing( EventArgs args )
		{
			base.OnTestInitializing( args );
			Container.RegisterInstance( HelloWorld );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void TestName()
		{
			Assert.AreEqual( HelloWorld, Subject.HelloWorld );
		}
	}
}
