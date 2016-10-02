using DragonSpark.ComponentModel;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.ComponentModel
{
	public class HostingAttributeBaseTests
	{
		[Theory, AutoData]
		public void Field( object item, int number )
		{
			var parameter = new FieldHostedAttribute( item, number ).Get( new object() );
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

		class FieldHostedAttribute : HostingAttributeBase
		{
			public FieldHostedAttribute(object item, int number) : base( p => new Target( item, number ) ) {}
		}
	}
}