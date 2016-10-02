using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class MethodExtensions
	{
		public static T CreateDelegate<T>( this MethodInfo @this ) => @this.CreateDelegate( typeof(T) ).To<T>();

		public static MethodInfo AccountForGenericDefinition( this MethodInfo @this ) => Account.Get( @this );
		readonly static IParameterizedSource<MethodInfo, MethodInfo> Account = new Cache<MethodInfo, MethodInfo>( AccountForGenericDefinitionBody );
		static MethodInfo AccountForGenericDefinitionBody( MethodInfo @this )
		{
			var result = @this.DeclaringType.IsConstructedGenericType ? @this.DeclaringType.GetGenericTypeDefinition().GetRuntimeMethods().SingleOrDefault( MethodEqualitySpecification.For( @this ) )
				:
				@this;
			return result;
		}

		public static MethodInfo LocateInDerivedType( this MethodInfo @this, Type derivedType ) => @this.DeclaringType != derivedType ? Locate( @this, derivedType ) : @this;

		static MethodInfo Locate( MethodInfo @this, Type derivedType ) => derivedType.GetRuntimeMethods().Introduce( @this, tuple => MethodEqualitySpecification.For( tuple.Item1 )( tuple.Item2 ) ).SingleOrDefault();

		public static MethodInfo AccountForClosedDefinition( this MethodInfo @this, Type closedType )=> @this.ContainsGenericParameters ? @this.LocateInDerivedType( closedType ) : @this;
	}
}