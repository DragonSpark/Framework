using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using DragonSpark.Activation;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup
{
	public class InjectionFactoryFactory : InjectionMemberFactory<InjectionFactory>
	{
		readonly Type factoryType;
		readonly object parameter;

		public InjectionFactoryFactory( Type factoryType, object parameter )
		{
			this.factoryType = factoryType;
			this.parameter = parameter;
		}

		protected override InjectionFactory CreateItem( InjectionMemberContext context )
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
			var context = new ObjectFactoryParameter( factoryType, parameter ?? type );
			var result = Activator.Current.Activate<FactoryBuiltObjectFactory>().Create( context );
			return result;
		}
	}
}