using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized
{
	public class ExtensionsTests
	{
		[Fact]
		public void Coverage()
		{
			var instance = new object();
			Assert.Contains( instance, Sources.Default.GetImmutable( instance ) );
			Assert.Same( instance, Source.Default.GetImmutable( instance.Yield().ToImmutableArray() ) );

			Assert.Null( Source.Default.Get() );
			Assert.Same( instance, Source.Default.Get( instance ) );
			Assert.Null( Immutable.Default.Get() );
			Assert.Same( instance, Immutable.Default.Get( instance ) );
			Assert.Same( instance, Source.Default.Apply( SelfAlteration<object>.Default ).Get( instance ) );
			Assert.Same( instance, Sources.Default.Apply( SelfAlteration<object>.Default ).Get( instance ).Only() );
		}

		sealed class Sources : ParameterizedSourceBase<object, IEnumerable<object>>
		{
			public static Sources Default { get; } = new Sources();
			Sources() {}

			public override IEnumerable<object> Get( object parameter ) => parameter.Yield();
		}

		sealed class Source : ParameterizedSourceBase<IEnumerable<object>, object>
		{
			public static Source Default { get; } = new Source();
			Source() {}

			public override object Get( IEnumerable<object> parameter ) => parameter.Only();
		}

		sealed class Immutable : ParameterizedSourceBase<ImmutableArray<object>, object>
		{
			public static Immutable Default { get; } = new Immutable();
			Immutable() {}

			public override object Get( ImmutableArray<object> parameter ) => parameter.AsEnumerable().Only();
		}


	}
}