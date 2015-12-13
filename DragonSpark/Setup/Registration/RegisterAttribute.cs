using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;

namespace DragonSpark.Setup.Registration
{
	public sealed class RegisterAttribute : RegistrationBaseAttribute
	{
		public RegisterAttribute() : this( null, null )
		{}

		public RegisterAttribute( Type @as ) : this( @as, null )
		{}

		public RegisterAttribute( string name ) : this( null, name )
		{}

		RegisterAttribute( Type @as, string name ) : base( new RegistrationByConvention( @as, name ) )
		{}
	}

	public class RegistrationByConvention : IConventionRegistration
	{
		readonly Type @as;
		readonly string name;

		public RegistrationByConvention( Type @as = null, string name = null )
		{
			this.@as = @as;
			this.name = name;
		}

		public void Register( IServiceRegistry registry, Type subject )
		{
			var from = @as ?? subject.Adapt().GetConventionCandidate();
			registry.Register( from, subject, @name );
		}
	}
}