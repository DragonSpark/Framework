using DragonSpark.Aspects.Build;
using DragonSpark.Sources;
using System;

namespace DragonSpark.Aspects
{
	public class TypeDefinition : ItemSource<IMethodStore>, ITypeDefinition
	{
		public TypeDefinition( Type declaringType, params IMethodStore[] methods ) : base( methods )
		{
			DeclaringType = declaringType;
		}

		public Type DeclaringType { get; }
	}
}