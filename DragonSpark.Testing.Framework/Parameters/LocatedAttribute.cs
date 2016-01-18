using System;
using System.Linq;
using System.Reflection;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup;
using Microsoft.Practices.Unity.Utility;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit2;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Testing.Framework.Parameters
{
	[AttributeUsage( AttributeTargets.Parameter )]
	public class LocatedAttribute : CustomizeAttribute
	{
		readonly bool enabled;

		public LocatedAttribute( bool enabled = true )
		{
			this.enabled = enabled;
		}

		public override ICustomization GetCustomization( ParameterInfo parameter ) => new ConfigureLocationCustomization( parameter.ParameterType, enabled );
	}

	[AttributeUsage( AttributeTargets.Parameter )]
	public class EnsureValuesAttribute : CustomizeAttribute
	{
		class Customization : ICustomization
		{
			readonly Type requestType;

			public Customization( [Required]Type requestType )
			{
				this.requestType = requestType;
			}

			public void Customize( IFixture fixture ) => fixture.Behaviors.Add( new Builder( requestType ) );

			class Builder : ISpecimenBuilderTransformation, ISpecimenCommand
			{
				readonly Type type;

				public Builder( Type type )
				{
					this.type = type;
				}

				public ISpecimenBuilder Transform( ISpecimenBuilder builder ) => new Postprocessor( builder, this );

				public void Execute( object specimen, ISpecimenContext context )
				{
					if ( type.IsInstanceOfType( specimen ) )
					{
						specimen.GetType().GetPropertiesHierarchical().Where( DefaultValuePropertySpecification.Instance.IsSatisfiedBy ).Each( info =>
						{
							info.GetValue( specimen );
						} );
					}
				}
			}
		}

		public override ICustomization GetCustomization( ParameterInfo parameter ) => new Customization( parameter.ParameterType );
	}
}