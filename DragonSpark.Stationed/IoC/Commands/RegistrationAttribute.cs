using System;

namespace DragonSpark.Common.IoC.Commands
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public class RegistrationAttribute : Attribute
	{
		public RegistrationAttribute( Priority priority )
		{
			Priority = priority;
		}

		public Priority Priority { get; set; }
	}
}