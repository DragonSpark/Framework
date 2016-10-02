using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	sealed class MethodEqualitySpecification : SpecificationWithContextBase<MethodInfo>
	{
		public static Func<MethodInfo, Func<MethodInfo, bool>> For { get; } = new Cache<MethodInfo, Func<MethodInfo, bool>>( info => new MethodEqualitySpecification( info ).ToSpecificationDelegate() ).ToDelegate();

		readonly Func<Type, Type> map;

		MethodEqualitySpecification( MethodInfo context ) : base( context )
		{
			map = Map;
		}

		public override bool IsSatisfiedBy( MethodInfo parameter ) => 
			Equals( parameter, Context ) || parameter.Name == Context.Name && Map( parameter.ReturnType ) == Context.ReturnType && parameter.GetParameterTypes().Select( map ).SequenceEqual( Context.GetParameterTypes().AsEnumerable() );

		Type Map( Type type )
		{
			var result = type.IsGenericParameter ? Context.DeclaringType.GenericTypeArguments[type.GenericParameterPosition] : type.GetTypeInfo().ContainsGenericParameters ? 
					type.GetGenericTypeDefinition().MakeGenericType( type.GenericTypeArguments.Select( map ).ToArray() ) : type;
			return result;
		}
	}
}