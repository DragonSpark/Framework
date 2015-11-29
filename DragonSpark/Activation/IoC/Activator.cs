using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Activation.IoC
{
	class Activator : IActivator
	{
		readonly IUnityContainer container;
		readonly IResolutionSupport support;

		public Activator( IUnityContainer container, IResolutionSupport support )
		{
			this.container = container;
			this.support = support;
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
					x.As<TypedInjectionValue>( parameterValue =>
					{
						child.RegisterInstance( parameterValue.ParameterType, parameterValue.GetResolverPolicy( null ).Resolve( null ) );
					});

					child.RegisterAllClasses( x );
				} );

				var result = new ResolutionContext( child.DetermineLogger() ).Execute( () => child.Resolve( type ) );
				return result;
			}
		}
	}
}