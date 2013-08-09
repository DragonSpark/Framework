using DragonSpark.IoC;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.IoC.Configuration;
using DragonSpark.Testing.TestObjects.IoC;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DragonSpark.Testing.IoC
{
	/// <summary>
	/// Summary description for DragonSparkExtensionTests
	/// </summary>
	[TestClass]
	public class DragonSparkExtensionTests
	{
		[TestMethod]
		public void VerifyINamedObjectRegisteredCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			Assert.IsTrue( unityContainer.IsRegistered<INamedObject>() );
		}

		[TestMethod]
		public void VerifyNamedObjectRegisteredCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var isRegistered = unityContainer.IsRegisteredOrMapped<NamedObject>();
			Assert.IsTrue( isRegistered );
		}

		[TestMethod]
		public void VerifyNamedTypeObjectRegisteredCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			Assert.IsTrue( unityContainer.IsRegisteredOrMapped<INamedTypeObject>( "Default" ) );
		}

		[TestMethod]
		public void VerifyNamedNamedTypeObjectRegisteredCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			Assert.IsTrue( unityContainer.IsRegisteredOrMapped<NamedTypeObject>( "Normal" ) );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyUnRegisteredTypeReturnsNull()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			Type type = unityContainer.ResolveType<object>();
			Assert.IsNull( type );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyRegisteredTypeReturnsSelf()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			Type type = unityContainer.ResolveType<NamedObject>();
			Assert.AreEqual( typeof(NamedObject), type );
		}


		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyTypeResolvesCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			Type type = unityContainer.ResolveType<INamedObject>();
			Assert.AreEqual( typeof(NamedObject), type );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyNamedTypeResolvesCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			Type type = unityContainer.ResolveType<INamedTypeObject>( "Default" );
			Assert.AreEqual( typeof(NamedTypeObject), type );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyMappingsResolveCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var types = unityContainer.ResolveMappings<NamedObject>().ToArray();
			Assert.AreEqual( 1, types.Length );
			Assert.AreEqual( typeof(INamedObject), types[0] );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyNamedMappingsResolveCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var types = unityContainer.ResolveMappings<NamedTypeObject>( "Default" ).ToArray();
			Assert.AreEqual( 1, types.Length );
			Assert.AreEqual( typeof(INamedTypeObject), types[0] );
		}

	}
}
