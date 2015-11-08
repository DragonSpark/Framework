using System;
using System.Linq;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup
{
	public class InjectionFactoryFactory : InjectionMemberFactory<InjectionFactory>
	{
		readonly IFactory factory;
		readonly object parameter;

		public InjectionFactoryFactory( IFactory factory, object parameter )
		{
			this.factory = factory;
			this.parameter = parameter;
		}

		protected override InjectionFactory CreateFrom( Type resultType, InjectionMemberContext context )
		{
			var previous = context.Container.Registrations.FirstOrDefault( x => x.RegisteredType == context.TargetType && x.MappedToType != x.RegisteredType ).Transform( x => x.MappedToType );

			var result = new InjectionFactory( ( unityContainer, type, buildName ) =>
			{
				var item = Create( unityContainer, type, buildName ) ?? previous.Transform( x => context.Container.Resolve( x ) );
				return item;
			} );
			return result;
		}

		protected virtual object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = factory.Create( type, parameter );
			return result;
		}
	}
}