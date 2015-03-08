using System;
using System.Diagnostics.Contracts;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public static class NamedTypeBuildKeyExtensions
	{
		public static TResultType Create<TResultType>( this NamedTypeBuildKey key, IBuilderContext context )
		{
			Contract.Requires( key != null );
			Contract.Requires( context != null );

			var result = context.NewBuildUp( key.Transform( item => item.Instance, Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey.Make<TResultType> ) ).To<TResultType>();
			return result;
		}

		public static object Create( this NamedTypeBuildKey key, IUnityContainer container, Type type = null )
		{
			Contract.Requires( key != null );
			Contract.Requires( container != null );

			var result = key.Create<object>( container, type );
			return result;
		}

		public static TResultType Create<TResultType>( this NamedTypeBuildKey key, IUnityContainer container, Type type = null )
		{
			Contract.Requires( key != null );
			Contract.Requires( container != null );

			using ( var child = container.CreateChildContainer() )
			{
				var result = child.Resolve( key.Transform( x => x.BuildType, () => type ), key.Transform( x => x.BuildName ) ).To<TResultType>();
				return result;
			}
		}
	}
}