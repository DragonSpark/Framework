using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using Type = System.Type;

namespace DragonSpark.Setup.Registration
{
	[AttributeUsage( AttributeTargets.Class )]
	public abstract class RegistrationBaseAttribute : HostingAttribute
	{
		protected RegistrationBaseAttribute( Func<Type, IRegistration> factory ) : base( x => x.AsTo( factory ) ) {}
	}
}