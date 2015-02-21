using System;

namespace DragonSpark.Activation.IoC.Commands
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class RegistrationAttribute : Attribute
	{
		public RegistrationAttribute( Priority priority )
		{
			Priority = priority;
		}

		public Priority Priority { get; private set; }
	}
}