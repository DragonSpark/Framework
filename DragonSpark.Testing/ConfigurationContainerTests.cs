using DragonSpark.Objects;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Objects;
using DragonSpark.Testing.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DragonSpark.Testing
{
	/// <summary>
	/// Summary description for ConfigurationContainerTests
	/// </summary>
	[TestClass]
	public class ConfigurationContainerTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureXamlObjectSerializesCorrectly()
		{
			var container = new ApplicationConfiguration();
			var item = container.Instance.GetInstance<DragonSparkObject>( "DragonSparkKey" );
			Assert.IsNotNull( item );
			
			var expected = "DragonSpark Name";
			Assert.AreEqual( expected, item.Name );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureXamlDefaultObjectSerializesCorrectly()
		{
			var container = new ApplicationConfiguration();
			var item = container.Instance.GetInstance<DragonSparkObject>();
			Assert.IsNotNull( item );
		}
		
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyGetAllWithNonGenericWorks()
		{
			var container = new ApplicationConfiguration();
			var all = container.Instance.GetAllInstances( typeof(DragonSparkObject) );
			Assert.AreEqual( 3, all.Count() );
		}
	}
}