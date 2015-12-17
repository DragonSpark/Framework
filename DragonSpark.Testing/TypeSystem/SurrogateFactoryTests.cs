using DragonSpark.TypeSystem;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class SurrogateFactoryTests
	{
		[Theory, AutoData]
		public void Field( object item, int number )
		{
			var parameter = new SurrogateFactory().Create( new SurrogateFactory.Parameter( new FieldSurrogateAttribute( item, number ), typeof(Target) ) );
			var target = Assert.IsType<Target>( parameter );
			Assert.Equal( item, target.Item );
			Assert.Equal( number, target.Number );
		}

		[Theory, AutoData]
		public void Property( object item, int number )
		{
			var parameter = new SurrogateFactory().Create( new SurrogateFactory.Parameter( new PropertySurrogateAttribute( item, number ), typeof(Target) ) );
			var target = Assert.IsType<Target>( parameter );
			Assert.Equal( item, target.Item );
			Assert.Equal( number, target.Number );
		}

		public class Target
		{
			public Target( object item, int number )
			{
				Item = item;
				Number = number;
			}

			public object Item { get; }
			public int Number { get; }
		}

		class FieldSurrogateAttribute : SurrogateAttribute
		{
			readonly object item;
			readonly int number;

			public FieldSurrogateAttribute(object item, int number) : base( typeof(Target) )
			{
				this.item = item;
				this.number = number;
			}
		}

		class PropertySurrogateAttribute : SurrogateAttribute
		{
			public PropertySurrogateAttribute(object item, int number) : base( typeof(Target) )
			{
				Item = item;
				Number = number;
			}

			public object Item { get; }
			public int Number { get; }
		}
	}
}