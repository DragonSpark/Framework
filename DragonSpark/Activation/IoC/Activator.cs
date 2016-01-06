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

		public Activator( IUnityContainer container, IResolutionSupport support, RegistrationSupport registration )
		{
			this.container = container;
			this.support = support;
			this.registration = registration;
		}

		public bool CanActivate( Type type, string name = null ) => support.CanResolve( type, name );

		public object Activate( Type type, string name = null ) => container.Resolve( type, name );

		public bool CanConstruct( Type type, params object[] parameters ) => support.CanResolve( type, null, parameters );

		public object Construct( Type type, params object[] parameters )
		{
			using ( var child = container.CreateChildContainer() )
			{
				parameters.NotNull().Each( x => registration.AllClasses( x ) );

				var result = new ResolutionContext( child.Logger() ).Execute( () => child.Resolve( type ) );
				return result;
			}
		}
	}
}