using System;

namespace DragonSpark.Activation.IoC.Commands
{
	[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
	public sealed class SkipRegistrationsForAttribute : Attribute
	{
		readonly Type[] types;

		public SkipRegistrationsForAttribute( params Type[] types )
		{
			this.types = types;
		}

		public Type[] Types
		{
			get { return types; }
		}
	}
}