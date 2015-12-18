using System;

namespace DragonSpark.TypeSystem
{
	[AttributeUsage( AttributeTargets.Class )]
	public class SurrogateForAttribute : Attribute
	{
		public SurrogateForAttribute( Type type )
		{
			Type = type;
		}

		public Type Type { get; }
	}
}