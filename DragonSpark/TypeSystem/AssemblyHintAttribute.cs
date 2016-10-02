using System;

namespace DragonSpark.TypeSystem
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class AssemblyHintAttribute : Attribute
	{
		public AssemblyHintAttribute( string hint )
		{
			Hint = hint;
		}

		public string Hint { get; }
	}
}