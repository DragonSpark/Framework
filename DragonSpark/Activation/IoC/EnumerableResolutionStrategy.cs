using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace DragonSpark.Activation.IoC
{
	public class EnumerableResolutionStrategy : BuilderStrategy
	{
		delegate object Resolver( IBuilderContext context );

		static readonly MethodInfo GenericResolveArrayMethod = typeof(EnumerableResolutionStrategy).GetTypeInfo().DeclaredMethods.First( m => m.Name == nameof(Resolve) && !m.IsPublic && m.IsStatic );

		public override void PreBuildUp( IBuilderContext context )
		{
			Guard.ArgumentNotNull( context, nameof(context) );
			var type = context.BuildKey.Type;
			var info = type.GetTypeInfo();
			if ( info.IsGenericType && info.GetGenericTypeDefinition() == typeof(IEnumerable<>) )
			{
				var resolver = (Resolver)GenericResolveArrayMethod.MakeGenericMethod( type.Extend().GetEnumerableType() ).CreateDelegate( typeof(Resolver) );

				context.Existing = resolver( context );
				context.BuildComplete = true;
			}
		}

		static object Resolve<T>( IBuilderContext context )
		{
			var result = context.Policies.Get<IRegisteredNamesPolicy>( null )
				.Transform( policy => policy.GetRegisteredNames( typeof(T) )
					.Concat( new string[] { null } ).Concat( typeof(T).GetTypeInfo().IsGenericType ? policy.GetRegisteredNames( typeof(T).GetGenericTypeDefinition() ) : Enumerable.Empty<string>() )
					.Distinct()
					.Select( context.NewBuildUp<T> )
					.ToArray() 
				) ?? new T[0];
			return result;
		}
	}
}
