using System.IO;
using DragonSpark.Configuration;
using DragonSpark.Serialization;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing
{
	[TestClass]
	public class FileConfigurationContainerTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond ), DeploymentItem( "Resources/FileConfiguration.xaml" )]
		public void EnsureXamlObjectSerializesCorrectly()
		{
			var container = XamlSerializationSupport.Load<ConfigurationDictionary>( new FileStreamResolver( "FileConfiguration.xaml" ) );
			var item = container.Instance.GetInstance<DragonSparkObject>( "DragonSparkKey" );
			Assert.IsNotNull( item );
			
			var expected = "DragonSpark Name";
			Assert.AreEqual( expected, item.Name );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond ), DeploymentItem( "Resources/FileConfiguration.xaml" )]
		public void EnsureXamlStringObjectSerializesCorrectly()
		{
			var xaml = File.ReadAllText( "FileConfiguration.xaml" );
			var container = XamlSerializationSupport.Parse<ConfigurationDictionary>( xaml );
			var item = container.Instance.GetInstance<DragonSparkObject>( "DragonSparkKey" );
			Assert.IsNotNull( item );
			
			var expected = "DragonSpark Name";
			Assert.AreEqual( expected, item.Name );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureResourceXamlObjectSerializesCorrectly()
		{
			var container = XamlSerializationSupport.Load<ConfigurationDictionary>( new AssemblyStreamResolver( GetType().Assembly, "DragonSpark.Testing.Resources.ResourceConfiguration.xaml" ) );
			var item = container.Instance.GetInstance<DragonSparkObject>( "DragonSparkKey" );
			Assert.IsNotNull( item );
			
			var expected = "DragonSpark Name";
			Assert.AreEqual( expected, item.Name );
		}
	}
}