using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Windows.Runtime;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework
{
	public class ServiceLocator : Activation.IoC.ServiceLocator
	{
		public ServiceLocator() : this( FixtureContext.GetCurrent().GetLogger() )
		{}

		public ServiceLocator( IRecordingLogger logger ) : base( logger )
		{
			Container.RegisterInstance( logger );
			Container.EnsureExtension<FixtureExtension>();
		}
	}


	public class ServiceLocationCustomization : ICustomization
	{
		readonly ISpecimenBuilder builder;

		public ServiceLocationCustomization() : this( new ServiceLocator() )
		{}

		public ServiceLocationCustomization( IServiceLocator locator ) : this( locator, new ServiceLocationRelay( locator ) )
		{}

		public ServiceLocationCustomization( IServiceLocator locator, ISpecimenBuilder builder )
		{
			Locator = locator;
			this.builder = builder;
		}

		public void Customize( IFixture fixture )
		{
			fixture.GetCustomizations().Add( this );

			Locator.Register( fixture );
			fixture.ResidueCollectors.Add( builder );
		}

		public IServiceLocator Locator { get; }
	}

	public class CanLocateSpecification : IRequestSpecification
	{
		readonly IServiceLocator locator;

		public CanLocateSpecification( IServiceLocator locator )
		{
			this.locator = locator;
		}

		public bool IsSatisfiedBy( object request )
		{
			var result = request.AsTo<Type, bool>( type => locator.GetInstance<IActivator>().Transform( activator => activator.CanActivate( type ) ) );
			return result;
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

	class LocationActivator : IActivator
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
	}

	public class ServiceLocationRelay : ISpecimenBuilder
	{
		readonly IServiceLocator locator;
		readonly IRequestSpecification specification;

		public ServiceLocationRelay( IServiceLocator locator ) : this( locator, new CanLocateSpecification( locator ) )
		{}

		public ServiceLocationRelay( IServiceLocator locator, IRequestSpecification specification )
		{
			this.locator = locator;
			this.specification = specification;
		}

		public object Create( object request, ISpecimenContext context )
		{
			var result = specification.IsSatisfiedBy( request ) ? request.AsTo<Type, object>( locator.GetService ) : new NoSpecimen( request );
			return result;
		}
	}

	public class FixtureExtension : UnityContainerExtension
	{
		public class FixtureStrategy : BuilderStrategy
		{
			public override void PreBuildUp( IBuilderContext context )
			{
				if ( !context.BuildComplete && context.Existing == null )
				{
					var fixture = context.NewBuildUp<IFixture>();
					context.Existing = fixture.TryCreate<object>( context.BuildKey.Type );
					context.BuildComplete = context.Existing != null;
				}
				base.PreBuildUp( context );
			}
		}

		protected override void Initialize()
		{
			var activator = new LocationActivator( Container.Resolve<IActivator>(), typeof(ILogger), typeof(IActivator) );
			Container.RegisterInstance<IActivator>( activator );
			Context.Strategies.AddNew<FixtureStrategy>( UnityBuildStage.Creation );
		}
	}

	public class AssignLocationCustomization : ICustomization, IAfterTestAware
	{
		readonly AmbientLocatorKeyFactory factory;
		readonly IServiceLocation location;

		public AssignLocationCustomization() : this( ServiceLocation.Instance )
		{}

		public AssignLocationCustomization( IServiceLocation location ) : this( location, AmbientLocatorKeyFactory.Instance )
		{}

		public AssignLocationCustomization( IServiceLocation location, AmbientLocatorKeyFactory factory )
		{
			this.factory = factory;
			this.location = location;
		}

		public void Customize( IFixture fixture )
		{
			var locator = fixture.GetLocator();
			var key = factory.Create( fixture.GetCurrentMethod() );
			locator.Register( key );
			locator.Register( location );
			location.Assign( locator );
		}

		public void After( IFixture fixture, MethodInfo methodUnderTest )
		{
			location.Assign( null );
		}
	}

	public class FromCustomization : CustomizeAttribute
	{
		class Customization : ICustomization
		{
			public static Customization Instance { get; } = new Customization();

			public void Customize( IFixture fixture )
			{
				fixture.Freeze( Services.Location.Locator );
			}
		}

		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			var result = Customization.Instance;
			return result;
		}
	}

	public class AssignAttribute : CustomizeAttribute
	{
		class Customization : ICustomization
		{
			readonly Type type;

			public Customization( Type type )
			{
				this.type = type;
			}

			public void Customize( IFixture fixture )
			{
				var locator = (IServiceLocator)new SpecimenContext(fixture).Resolve(type);
				var location = fixture.Create<IServiceLocation>();
				location.Assign( locator );
			}
		}

		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			var result = new Customization( parameter.ParameterType );
			return result;
		}
	}

	/*public class DefaultAssignLocationAttribute : AssignLocationCustomization
	{
		public DefaultAssignLocationAttribute() : base( Activation.ServiceLocation.Instance )
		{}
	}*/


	/*class FixtureRegistry : IServiceRegistry
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
	}*/
}