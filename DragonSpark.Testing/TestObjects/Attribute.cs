using System;

namespace DragonSpark.Testing.TestObjects
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	class Attribute : System.Attribute
	{
		public string PropertyName { get; set; } 
	}
}