using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Activation.IoC
{
	class Activator : IActivator
	{
		readonly IUnityContainer container;
		readonly IResolutionSupport support;
		readonly RegistrationSupport registration;

		/*public Activator( IUnityContainer container, IResolutionSupport support ) : this( container, support, container.Resolve<RegistrationSupport>() )
		{}*/

		public Activator( IUnityContainer container, IResolutionSupport support, RegistrationSupport registration )
		{
			this.container = container;
			this.support = support;
			this.registration = registration;
		}

		public bool CanActivate( Type type, string name = null )
		{
			return support.CanResolve( type, name );
		}

		public object Activate( Type type, string name = null )
		{
			var result = container.Resolve( type, name );
			return result;
		}

		public bool CanConstruct( Type type, params object[] parameters )
		{
			var result = support.CanResolve( type, null, parameters );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			using ( var child = container.CreateChildContainer() )
			{
				parameters.NotNull().Each( x => 
				{
					/*x.As<TypedInjectionValue>( parameterValue =>
					{
						var instance = parameterValue.GetResolverPolicy( null ).Resolve( null );
						child.RegisterInstance( parameterValue.ParameterType, instance );
					} );*/

					registration.AllClasses( x );
				} );

				var result = new ResolutionContext( child.Logger() ).Execute( () => child.Resolve( type ) );
				return result;
			}
		}
	}
}