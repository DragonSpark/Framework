using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Defaults = DragonSpark.Activation.Location.Defaults;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class ServicesCustomization : CustomizationBase
	{
		public static ServicesCustomization Default { get; } = new ServicesCustomization();
		ServicesCustomization() {}

		protected override void OnCustomize( IFixture fixture ) => fixture.Customizations.Insert( 0, FrameworkSpecimenBuilder.Implementation );

		sealed class FrameworkSpecimenBuilder : ISpecimenBuilder
		{
			public static FrameworkSpecimenBuilder Implementation { get; } = new FrameworkSpecimenBuilder();
			FrameworkSpecimenBuilder() : this( typeof(Type[]), typeof(Assembly[]), typeof(ImmutableArray<Type>), typeof(ImmutableArray<Assembly>) ) {}

			readonly Type[] types;

			[UsedImplicitly]
			public FrameworkSpecimenBuilder( params Type[] types )
			{
				this.types = types;
			}

			public object Create( object request, ISpecimenContext context )
			{
				var type = TypeCoercer.Default.Get( request );
				var result = type != null && types.Contains( type ) ? Defaults.ServiceSource( type ) : new NoSpecimen();
				return result;
			}
		}
	}
}