using System.Reflection;
using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Testing.Framework.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	public class ServiceLocationCustomization : ICustomization, IAfterTestAware
	{
		[Activate]
		public IServiceLocator Locator { get; set; }

		[Activate]
		public IServiceLocation Location { get; set; }

		[Activate]
		public IServiceLocationAuthority Authority { get; set; }

		public void Customize( IFixture fixture )
		{
			fixture.Items().Add( this );

			var relay = new ServiceLocationRelay( Locator, new AuthorizedLocationSpecification( Locator, Authority ) );
			fixture.ResidueCollectors.Add( relay );
		}

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