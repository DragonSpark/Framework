using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.IoC.Build
{
	public class DefaultValueStrategy : BuilderStrategy
	{
		public override void PreBuildUp(IBuilderContext context)
		{
			typeof(IDefaultValueProvider).IsAssignableFrom( context.BuildKey.Type ).IsFalse( () =>
			{
				var item = context.NewBuildUp<IDefaultValueProvider>();
				context.Existing = context.Existing.With( item.Apply );
			});
		}
	}

	public class EnumerableResolutionStrategy : BuilderStrategy
	{
		delegate object Resolver( IBuilderContext context );

		static readonly MethodInfo GenericResolveArrayMethod = typeof(EnumerableResolutionStrategy).GetTypeInfo().DeclaredMethods.First( m => m.Name == nameof(Resolve) && !m.IsPublic && m.IsStatic );

		public override void PreBuildUp( IBuilderContext context )
		{
			Guard.ArgumentNotNull( context, nameof(context) );
			var elementType = context.BuildKey.Type.GetEnumerableType();
			if ( elementType != null )
			{
				var resolver = (Resolver)GenericResolveArrayMethod.MakeGenericMethod( elementType ).CreateDelegate( typeof(Resolver) );

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