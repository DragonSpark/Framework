using System;
using DragonSpark.IoC;
using DragonSpark.Objects;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InjectionProperty=Microsoft.Practices.Unity.InjectionProperty;
using UnityContainer=Microsoft.Practices.Unity.UnityContainer;

namespace DragonSpark.Testing
{
	[TestClass]
	public class FactoryTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyObjectCreatedCorrectly()
		{
			var locator = ResolveLocator();
			ServiceLocator.SetLocatorProvider( () => locator );

			var creator = new Factory<ObjectCreationTarget>();
			var item = creator.Create();
			Assert.AreNotEqual( Guid.Empty, item.ID );
			Assert.IsNotNull( item.DragonSparkObject );
			Assert.IsNotNull( item.Instance );
			Assert.AreEqual( item.Instance.Name, "My Name" );
		}

		static IServiceLocator ResolveLocator()
		{
			var container = new UnityContainer()
				.AddNewExtension<DragonSparkExtension>()
				.RegisterType<DragonSparkObject>( "Instance", new InjectionProperty( "Name", "My Name" ) );
			var result = new DragonSpark.IoC.UnityServiceLocator( container );
			return result;
		}
	}
}