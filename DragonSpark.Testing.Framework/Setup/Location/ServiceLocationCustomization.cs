using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	/*public class ObjectBuilderCustomization : CustomizationBase
	{
		[Activate]
		public SetupAutoData Context { get; set; }

		protected override void Customize( IFixture fixture )
		{
			var specification = new BuilderSpecification( Context.Method.GetParameters().Select( info => info.ParameterType ).ToArray() );
			fixture.Behaviors.Add( new ObjectBuilderBehavior( specification ) );
		}

		class BuilderSpecification : SpecificationBase<Type>
		{
			readonly Type[] typesToBuild;

			public BuilderSpecification( Type[] typesToBuild )
			{
				this.typesToBuild = typesToBuild;
			}

			protected override bool IsSatisfiedByParameter( Type parameter ) => base.IsSatisfiedByParameter( parameter ) && !typeof(Mock).Adapt().IsAssignableFrom( parameter ) && typesToBuild.Contains( parameter );
		}

		class ObjectBuilderBehavior : ISpecimenBuilderTransformation
		{
			readonly ISpecification<Type> specification;
			
			public ObjectBuilderBehavior( ISpecification<Type> specification )
			{
				this.specification = specification;
			}

			public ISpecimenBuilder Transform( ISpecimenBuilder builder ) => new Builder( builder, specification );

			class Builder : ISpecimenBuilderNode
			{
				readonly ISpecimenBuilder inner;
				readonly ISpecification<Type> specification;
				
				public Builder( ISpecimenBuilder inner, ISpecification<Type> specification )
				{
					this.inner = inner;
					this.specification = specification;
				}

				public ISpecimenBuilderNode Compose( IEnumerable<ISpecimenBuilder> builders )
				{
					var builder = builders.Fixed().With( b => b.Only() ?? new CompositeSpecimenBuilder( b ) );
					var result = new Builder( builder, specification );
					return result;
				}

				public object Create( object request, ISpecimenContext context )
				{
					var item = inner.Create( request, context );
					var result = request.AsTo<Type, object>( type => specification.IsSatisfiedBy( type ) ? item.BuildUp() : null ) ?? item;
					return result;
				}

				

				public IEnumerator<ISpecimenBuilder> GetEnumerator()
				{
					yield return inner;
				}

				IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
			}
		}
		
	}*/

	public abstract class CustomizationBase : ICustomization
	{
		[Aspects.BuildUp]
		void ICustomization.Customize( IFixture fixture )
		{
			Customize( fixture );
		}
		
		protected virtual void Customize( IFixture fixture )
		{}
	}

	public class ServiceLocationCustomization : CustomizationBase
	{
		[Activate]
		public IServiceLocator Locator { get; set; }

		[Activate]
		public IServiceLocationAuthority Authority { get; set; }

		protected override void Customize( IFixture fixture ) => new ServiceLocationRelay( Locator, new AuthorizedLocationSpecification( Locator, Authority ) ).With( fixture.ResidueCollectors.Add );
	}
	
	class FixtureRegistry : IServiceRegistry
	{
		readonly IFixture fixture;

		public FixtureRegistry( IFixture fixture )
		{
			this.fixture = fixture;
		}

		public void Register( Type @from, Type mappedTo, string name = null ) => fixture.Customizations.Add( new TypeRelay( @from, mappedTo ) );

		public void Register( Type type, object instance ) => this.InvokeGenericAction( nameof(RegisterInstance), new[] { type }, instance );

		public void RegisterInstance<T>( T instance ) => fixture.Customize<T>( c => c.FromFactory( () => instance ).OmitAutoProperties() );

		public void RegisterFactory( Type type, Func<object> factory ) => this.InvokeGenericAction( nameof(RegisterFactory), type.ToItem(), factory );

		public void RegisterFactory<T>( Func<object> factory ) => fixture.Customize<T>( c => c.FromFactory( () => (T)factory() ).OmitAutoProperties() );
	}
}