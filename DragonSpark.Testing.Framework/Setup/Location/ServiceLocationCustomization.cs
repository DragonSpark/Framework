using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Testing.Framework.Extensions;
using Microsoft.Practices.ServiceLocation;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	public class ObjectBuilderCustomization : ICustomization
	{
		public void Customize( IFixture fixture )
		{
			var specification = new BuilderSpecification( fixture.GetMethod().GetParameters().Select( info => info.ParameterType ).ToArray() );
			fixture.Behaviors.Add( new ObjectBuilderBehavior( specification ) );
		}

		class BuilderSpecification : SpecificationBase<Type>
		{
			readonly Type[] typesToBuild;

			public BuilderSpecification( Type[] typesToBuild )
			{
				this.typesToBuild = typesToBuild;
			}

			protected override bool IsSatisfiedByParameter( Type parameter )
			{
				return base.IsSatisfiedByParameter( parameter ) /*&& !parameter.IsInterface*/ && !typeof(Mock).Adapt().IsAssignableFrom( parameter ) && typesToBuild.Contains( parameter );
			}
		}

		class ObjectBuilderBehavior : ISpecimenBuilderTransformation
		{
			readonly ISpecification<Type> specification;
			
			public ObjectBuilderBehavior( ISpecification<Type> specification )
			{
				this.specification = specification;
			}

			public ISpecimenBuilder Transform( ISpecimenBuilder builder )
			{
				var result = new Builder( builder, specification );
				return result;
			}

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

				IEnumerator IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}
			}
		}
		
	}

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

	public class ServiceLocationCustomization : CustomizationBase, ITestExecutionAware
	{
		[Activate]
		public IServiceLocator Locator { get; set; }

		[Activate]
		public IServiceLocation Location { get; set; }

		[Activate]
		public IServiceLocationAuthority Authority { get; set; }

		protected override void Customize( IFixture fixture )
		{
			fixture.Items().Add( this );

			var relay = new ServiceLocationRelay( Locator, new AuthorizedLocationSpecification( Locator, Authority ) );
			fixture.ResidueCollectors.Add( relay );
		}

		void ITestExecutionAware.Before( IFixture fixture, MethodInfo methodUnderTest )
		{}

		public void After( IFixture fixture, MethodInfo methodUnderTest )
		{
			Location.Assign( null );
		}
	}

/*	class FixtureActivator : IActivator
	{
		readonly IFixture fixture;

		public FixtureActivator( IFixture fixture )
		{
			this.fixture = fixture;
		}

		public bool CanActivate( Type type, string name = null )
		{
			return true;
		}

		public object Activate( Type type, string name = null )
		{
			return new SpecimenContext( fixture ).Resolve( type );
		}

		public object Construct( Type type, params object[] parameters )
		{
			return Activate( type );
		}
	}*/

	/*class LocationActivator : IActivator
	{
		readonly IActivator activator;
		readonly Type[] passthrough;

		readonly IList<Type> activating = new List<Type>();

		public LocationActivator( IActivator activator, params Type[] passthrough )
		{
			this.activator = activator;
			this.passthrough = passthrough;
		}

		public bool CanActivate( Type type, string name = null )
		{
			var result = !activating.Concat( passthrough ).Contains( type ) && activator.CanActivate( type, name );
			return result;
		}

		public object Activate( Type type, string name = null )
		{
			var result = Execute( type, () => activator.Activate( type, name ) );
			return result;
		}

		object Execute( Type type, Func<object> factory )
		{
			activating.Add( type );
			using ( new DisposableActionContext( () => activating.Remove( type ) ) )
			{
				var result = factory();
				return result;
			}
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = Execute( type, () => activator.Construct( type, parameters ) );
			return result;
		}
	}*/

	/*public class FixtureExtension : UnityContainerExtension
	{
		public class FixtureStrategy : BuilderStrategy
		{
			public override void PreBuildUp( IBuilderContext context )
			{
				if ( !context.BuildComplete && context.Existing == null )
				{
					var fixture = context.NewBuildUp<IFixture>().Create();
					context.Existing = fixture.TryCreate<object>( context.BuildKey.Type );
					context.BuildComplete = context.Existing != null;
				}
				base.PreBuildUp( context );
			}
		}

		public class FixturePolicy : IBuildPlanPolicy
		{
			readonly IFixture fixture;

			public FixturePolicy( IFixture fixture )
			{
				this.fixture = fixture;
			}

			public void BuildUp( IBuilderContext context )
			{
				context.
			}
		}

		public void Each(  )

		protected override void Initialize()
		{
			Context.Strategies.AddNew<FixtureStrategy>( UnityBuildStage.Creation );
		}
	}*/

	class FixtureRegistry : IServiceRegistry
	{
		readonly IFixture fixture;

		public FixtureRegistry( IFixture fixture )
		{
			this.fixture = fixture;
		}

		public void Register( Type @from, Type mappedTo, string name = null )
		{
			fixture.Customizations.Add( new TypeRelay( @from, mappedTo ) );
		}

		public void Register( Type type, object instance )
		{
			this.InvokeGenericAction( nameof(RegisterInstance), new[] { type }, instance );
		}

		public void RegisterInstance<T>( T instance )
		{
			fixture.Customize<T>( c => c.FromFactory( () => instance ).OmitAutoProperties() );
		}

		public void RegisterFactory( Type type, Func<object> factory )
		{
			this.InvokeGenericAction( nameof(RegisterFactory), new[] { type }, factory );
		}

		public void RegisterFactory<T>( Func<object> factory )
		{
			fixture.Customize<T>( c => c.FromFactory( () => (T)factory() ).OmitAutoProperties() );
		}
	}
}