using DragonSpark.Testing.Framework;
using DragonSpark.Testing.IoC.Configuration;
using DragonSpark.Testing.TestObjects.IoC;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.IoC
{
	[TestClass]
	public class ConfigurationTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyConfigurationCreatesContainerCorrectly()
		{
			var container = new Core().Instance;
			Assert.IsNotNull( container );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyTypesRegisteredCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var item = unityContainer.Resolve<INamedObject>();
			Assert.IsNotNull( item );
			Assert.IsInstanceOfType( item, typeof(NamedObject) );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyInjectionPropertyRegisteredCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var item = unityContainer.Resolve<INamedObject>();
			Assert.AreEqual( "Mapped Named Object", item.Name );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyInjectionPropertyRegisteredCorrectlyWithType()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var item = unityContainer.Resolve<INamedTypeObject>( "Default" );
			Assert.AreEqual( "Mapped Named-Type Object", item.Name );
			Assert.AreEqual( typeof(string), item.Type );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyInjectionPropertyRegisteredCorrectlyWithIntType()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var item = unityContainer.Resolve<INamedTypeObject>( "Normal" );
			Assert.AreEqual( "Named-Type Object", item.Name );
			Assert.AreEqual( typeof(int), item.Type );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyInjectionPropertyForGenericPropertyRegisteredCorrectly()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var item = unityContainer.Resolve<GenericObject<INamedTypeObject>>();
			Assert.AreEqual( "Named-Type Object", item.Property.Name );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyConstructorInjection()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var item = unityContainer.Resolve<ConstructorObject>();
			Assert.AreEqual( "String-based Construction!", item.Message );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyConstructorInjectionWithInt()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var item = unityContainer.Resolve<ConstructorObject>( "Int" );
			Assert.AreEqual( 6776, item.Number );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyMethodInjection()
		{
			var unityContainer = new Core().Instance.Instance.GetInstance<IUnityContainer>();
			var item = unityContainer.Resolve<MethodObject>();
			Assert.AreEqual( "Hello Method Call!", item.Message );
		}
	}
}