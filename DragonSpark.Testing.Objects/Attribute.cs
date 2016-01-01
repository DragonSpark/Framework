using System;

namespace DragonSpark.Testing.Objects
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public class Attribute : System.Attribute
	{
		public string PropertyName { get; set; } 
	}
}