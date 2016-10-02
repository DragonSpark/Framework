using System;

namespace DragonSpark.Application
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class ApplicationAttribute : RegistrationAttribute
	{
		public ApplicationAttribute() : this( Priority.High ) {}
		public ApplicationAttribute( Priority priority ) : base( priority ) {}
	}
}