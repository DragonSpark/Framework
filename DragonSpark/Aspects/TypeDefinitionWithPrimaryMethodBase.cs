using DragonSpark.Aspects.Build;

namespace DragonSpark.Aspects
{
	public abstract class TypeDefinitionWithPrimaryMethodBase : TypeDefinition
	{
		protected TypeDefinitionWithPrimaryMethodBase( IMethodStore method ) : base( method.DeclaringType, method )
		{
			Method = method;
		}

		public IMethodStore Method { get; }
	}
}