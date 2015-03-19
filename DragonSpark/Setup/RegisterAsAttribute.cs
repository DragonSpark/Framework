using System;

namespace DragonSpark.Setup
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class RegisterAsAttribute : Attribute
	{
		readonly Type @as;

		public RegisterAsAttribute( Type @as )
		{
			this.@as = @as;
		}

		public Type As
		{
			get { return @as; }
		}
	}
}