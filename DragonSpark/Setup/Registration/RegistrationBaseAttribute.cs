using DragonSpark.TypeSystem;
using System;

namespace DragonSpark.Setup.Registration
{
	[AttributeUsage( AttributeTargets.Class )]
	public abstract class RegistrationBaseAttribute : HostingAttribute
	{
		protected RegistrationBaseAttribute( Func<IRegistration> factory ) : base( factory ) {}
	}
}