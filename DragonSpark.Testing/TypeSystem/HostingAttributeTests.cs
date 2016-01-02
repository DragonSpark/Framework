using DragonSpark.TypeSystem;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class HostingAttributeTests
	{
		[Theory, AutoData]
		public void Field( object item, int number )
		{
			var parameter = new FieldHostedAttribute( item, number ).Item;
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

		class FieldHostedAttribute : HostingAttribute
		{
			public FieldHostedAttribute(object item, int number) : this( () => new Target( item, number ) )
			{}

			FieldHostedAttribute( Func<object> p ) : base( p )
			{}
		}
	}
}