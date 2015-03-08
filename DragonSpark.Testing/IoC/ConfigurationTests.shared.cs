using DragonSpark.Testing.Framework;
using DragonSpark.Testing.IoC.Configuration;
using DragonSpark.Testing.TestObjects.IoC;
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
			var item = new Core().Instance.Instance.GetInstance<INamedObject>();
			Assert.IsNotNull( item );
			Assert.IsInstanceOfType( item, typeof(NamedObject) );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyInjectionPropertyRegisteredCorrectly()
		{
			var item = new Core().Instance.Instance.GetInstance<INamedObject>();
			Assert.AreEqual( "Mapped Named Object", item.Name );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyInjectionPropertyRegisteredCorrectlyWithType()
		{
			var item = new Core().Instance.Instance.GetInstance<INamedTypeObject>( "Default" );
			Assert.AreEqual( "Mapped Named-Type Object", item.Name );
			Assert.AreEqual( typeof(string), item.Type );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyInjectionPropertyRegisteredCorrectlyWithIntType()
		{
			var item = new Core().Instance.Instance.GetInstance<INamedTypeObject>( "Normal" );
			Assert.AreEqual( "Named-Type Object", item.Name );
			Assert.AreEqual( typeof(int), item.Type );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyInjectionPropertyForGenericPropertyRegisteredCorrectly()
		{
			var item = new Core().Instance.Instance.GetInstance<GenericObject<INamedTypeObject>>();
			Assert.AreEqual( "Named-Type Object", item.Property.Name );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyConstructorInjection()
		{
			var item = new Core().Instance.Instance.GetInstance<ConstructorObject>();
			Assert.AreEqual( "String-based Construction!", item.Message );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyConstructorInjectionWithInt()
		{
			var item = new Core().Instance.Instance.GetInstance<ConstructorObject>( "Int" );
			Assert.AreEqual( 6776, item.Number );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyMethodInjection()
		{
			var item = new Core().Instance.Instance.GetInstance<MethodObject>();
			Assert.AreEqual( "Hello Method Call!", item.Message );
		}
	}
}