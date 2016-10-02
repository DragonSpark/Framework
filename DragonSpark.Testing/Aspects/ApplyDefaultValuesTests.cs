using DragonSpark.ComponentModel;
using Ploeh.AutoFixture.Xunit2;
using System.ComponentModel;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class ApplyDefaultValuesTests
	{
		public class ValueHost
		{
			[DefaultValue( true )]
			public bool PropertyName { get; set; }

			[DefaultValue( 6776 )]
			public int Number { get; set; }
		}

		static class StaticValueHost
		{
			[DefaultValue( true )]
			public static bool PropertyName { get; set; }
		}

		[Fact]
		public void BasicAccess()
		{
			var sut = new ValueHost();
			var first = sut.PropertyName;
			Assert.True( first );

			Assert.Equal( 6776, sut.Number );

			var second = sut.PropertyName;
			Assert.True( second );

			var next = new ValueHost();
			Assert.True( next.PropertyName );

			var name = StaticValueHost.PropertyName;
			Assert.True( name );

			StaticValueHost.PropertyName = StaticValueHost.PropertyName;
		}

		[Theory, AutoData]
		public void Number( [NoAutoProperties]ValueHost sut, int number )
		{
			Assert.Equal( 6776, sut.Number );

			sut.Number = number;
			Assert.Equal( number, sut.Number );
		}

		[Fact]
		public void Build()
		{
			var sut = new BuildTarget();
			Assert.True( sut.Boolean );
			sut.Boolean = false;

			Assert.Null( sut.Value );
			sut.Call();
			Assert.False( sut.Boolean );
			Assert.Same( SkipFirstCallProvider.Default.Item, sut.Value );
		}

		public class BuildTarget
		{
			[DefaultValue( true )]
			public bool Boolean { get; set; }

			[SkipFirstCallValue]
			public object Value { get; set; }

			public void Call() => Value = SkipFirstCallProvider.Default.Item;
		}

		public sealed class SkipFirstCallValue : DefaultValueBase
		{
			public SkipFirstCallValue() : base( t => SkipFirstCallProvider.Default ) {}
		}

		public sealed class SkipFirstCallProvider : DefaultValueProviderBase
		{
			public static SkipFirstCallProvider Default { get; } = new SkipFirstCallProvider();

			readonly public object Item = new object();

			readonly ConditionMonitor monitor = new ConditionMonitor();

			public override object Get( DefaultValueParameter parameter ) => monitor.Apply() ? null : Item;
		}
	}
}