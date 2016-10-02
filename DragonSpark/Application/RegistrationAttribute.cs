using System;

namespace DragonSpark.Application
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public class RegistrationAttribute : PriorityAttribute
	{
		public RegistrationAttribute() : this( Priority.Normal ) {}
		public RegistrationAttribute( Priority priority ) : base( priority ) {}
	}
}