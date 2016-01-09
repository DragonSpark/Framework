using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace DragonSpark.Activation.IoC
{
	public class InjectionFactoryFactory : InjectionMemberFactory<InjectionFactory>
	{
		readonly Type factoryType;
		readonly object context;

		public InjectionFactoryFactory( Type factoryType, object context )
		{
			this.factoryType = factoryType;
			this.context = context;
		}

		protected override InjectionFactory CreateItem( InjectionMemberParameter parameter )
		{
			var previous = parameter.Container.Registrations.FirstOrDefault( x => x.RegisteredType == parameter.TargetType && x.MappedToType != x.RegisteredType ).With( x => x.MappedToType );

			var result = new InjectionFactory( ( unityContainer, type, buildName ) =>
			{
				var item = Factory.From( factoryType, context ?? type ) ?? previous.With( x => unityContainer.Resolve( x ) );
				return item;
			} );
			return result;
		}
	}
}