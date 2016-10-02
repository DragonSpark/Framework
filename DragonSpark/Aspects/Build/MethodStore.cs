using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;
using System.Reflection;

namespace DragonSpark.Aspects.Build
{
	public sealed class MethodStore : FactoryCache<Type, MethodInfo>, IMethodStore
	{
		readonly string methodName;

		public MethodStore( Type declaringType, string methodName ) : base( TypeAssignableSpecification.Defaults.Get( declaringType ) )
		{
			DeclaringType = declaringType;
			this.methodName = methodName;
		}

		public Type DeclaringType { get; }

		protected override MethodInfo Create( Type parameter )
		{
			var mapping = parameter.Adapt().GetMappedMethods( DeclaringType ).Introduce( methodName, tuple => tuple.Item1.InterfaceMethod.Name == tuple.Item2 ).Only();
			var result = mapping.MappedMethod?.LocateInDerivedType( parameter ).AccountForGenericDefinition();
			return result;
		}
	}
}