using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Xunit;

namespace DragonSpark.Testing.Activation.IoC
{
	public class InjectionFactoryFactoryTests
	{
		const string HelloWorld = "Hello World";

		[Fact]
		public void Simple()
		{
			var container = DragonSpark.Activation.FactoryModel.Factory.Create<UnityContainer>();
			var sut = new InjectionFactoryFactory( typeof(SimpleFactory) );
			var create = sut.Create( new InjectionMemberParameter( container, typeof(string) ) );
			container.RegisterType( typeof(string), create );
			Assert.Equal( HelloWorld, container.Resolve<string>() );
		} 

		[Fact]
		public void Create()
		{
			var container = DragonSpark.Activation.FactoryModel.Factory.Create<UnityContainer>();
			var sut = new InjectionFactoryFactory( typeof(Factory) );
			container.RegisterType<IItem, Item>( new ContainerControlledLifetimeManager() );
			var expected = container.Resolve<IItem>();
			var create = sut.Create( new InjectionMemberParameter( container, typeof(IItem) ) );
			container.RegisterType( typeof(IItem), create );
			Assert.Equal( expected, container.Resolve<IItem>() );
		} 

		class SimpleFactory : FactoryBase<string>
		{
			protected override string CreateItem() => HelloWorld;
		}

		class Factory : FactoryBase<IItem>
		{
			protected override IItem CreateItem() => null;
		}

		interface IItem
		{}

		class Item : IItem
		{}
	}
}