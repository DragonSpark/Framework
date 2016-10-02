using DragonSpark.Activation.Location;
using DragonSpark.Application.Setup;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Runtime;
using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	sealed class FixtureServiceProvider : DecoratedActivator
	{
		public FixtureServiceProvider( IFixture fixture ) : base( new Specification( fixture ), new Inner( fixture ).ToSourceDelegate() ) {}

		sealed class Inner : ParameterizedSourceBase<Type, object>
		{
			readonly IFixture fixture;

			public Inner( IFixture fixture )
			{
				this.fixture = fixture;
			}

			public override object Get( Type parameter ) => fixture.Create<object>( parameter );
		}

		sealed class Specification : SpecificationBase<Type>
		{
			readonly IServiceRepository registry;

			public Specification( IFixture fixture ) : this( AssociatedRegistry.Default.Get( fixture ) ) {}

			Specification( IServiceRepository registry )
			{
				this.registry = registry;
			}

			public override bool IsSatisfiedBy( Type parameter ) => registry.IsSatisfiedBy( parameter );
		}
	}
}