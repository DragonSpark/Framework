using System;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Setup.Registration
{
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