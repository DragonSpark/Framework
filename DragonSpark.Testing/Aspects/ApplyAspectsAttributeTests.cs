using DragonSpark.Aspects;
using DragonSpark.Sources.Coercion;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using JetBrains.Annotations;
using PostSharp.Patterns.Model;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class ApplyAspectsAttributeTests
	{
		[Fact]
		public void Verify()
		{
			Assert.Equal( 0, Source.Default.Get( 123 ) );
			Assert.Equal( 6776, Source.Default.Get( 6776 ) );

			var generalized = Source.Default.QueryInterface<IParameterizedSource<object, object>>();
			Assert.Equal( 6776, generalized.Get( "6776" ) );
			Assert.Equal( 0, generalized.Get( "123" ) );
		}

		[ApplyAspectsAttributeTests.ApplyAspects]
		class Source : GeneralizedParameterizedSourceBase<int, int>
		{
			public static Source Default { get; } = new Source();
			Source() {}

			public override int Get( int parameter ) => parameter;
		}

		public sealed class ApplyAspects : ApplyAspectsAttribute
		{
			public ApplyAspects() : base( typeof(Coercer), typeof(Specification) ) {}
		}

		sealed class Coercer : CoercerBase<string, int>
		{
			[UsedImplicitly]
			public static Coercer Default { get; } = new Coercer();
			Coercer() {}

			protected override int Coerce( string parameter ) => int.Parse( parameter );
		}

		sealed class Specification : SpecificationBase<int>
		{
			[UsedImplicitly]
			public static Specification Default { get; } = new Specification();
			Specification() {}

			public override bool IsSatisfiedBy( int parameter ) => parameter == 6776;
		}
	}
}