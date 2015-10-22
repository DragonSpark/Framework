using System;

namespace DragonSpark.Setup
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class RegisterAsAttribute : Attribute
	{
		public RegisterAsAttribute( Type @as ) : this( @as, null )
		{}

		public RegisterAsAttribute( Type @as, string name )
		{
			As = @as;
			Name = name;
		}

		public Type As { get; }
		public string Name { get; }
	}
}