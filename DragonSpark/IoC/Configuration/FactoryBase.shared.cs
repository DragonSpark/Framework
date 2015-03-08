using System;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public abstract class FactoryBase : InjectionMember, IFactory
	{
		public sealed override Microsoft.Practices.Unity.InjectionMember Create(IUnityContainer container, Type targetType)
		{
			var result = new Microsoft.Practices.Unity.InjectionFactory( Create );
			return result;
		}

		protected abstract object Create( IUnityContainer container, Type type, string buildName );

		object IFactory.Create( Type resultType, object source )
		{
			var container = source.As<IUnityContainer>() ?? source.As<IBuilderContext>().Transform( x => x.NewBuildUp<IUnityContainer>() );
			var result = container.Transform( y => Create( y, resultType, string.Empty ) );
			return result;
		}
	}
}