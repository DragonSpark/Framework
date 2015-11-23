using DragonSpark.Testing.TestObjects;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Framework
{
	public class TypesFactoryTests
	{
		class Types : Types<Class, IInterface, Derived>
		{}

		class Other : Types<Class, Other.More, Derived>
		{
			internal class Inner : Class { }

			internal class More : Types<Inner>
			{}
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void Basic( TypesFactory<Class> factory )
		{
			var instances = factory.Create( new[] { typeof(Types) } ).ToArray();
			Assert.Equal( 2, instances.Length );
			Assert.IsType<Class>( instances.First() );
			Assert.IsType<Derived>( instances.Last() );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void OtherInner( TypesFactory<Class> factory )
		{
			var instances = factory.Create( new[] { typeof(Other) } ).ToArray();
			Assert.Equal( 3, instances.Length );
			Assert.IsType<Class>( instances.First() );
			Assert.IsType<Other.Inner>( instances.ElementAt( 1 ) );
			Assert.IsType<Derived>( instances.Last() );
		}
	}
}
