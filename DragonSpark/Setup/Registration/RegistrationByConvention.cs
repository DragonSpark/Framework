using System;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Setup.Registration
{
	public class RegistrationByConvention : ConventionRegistration
	{
		readonly Type @as;

		public RegistrationByConvention( Type @as, string name )
		{
			this.@as = @as;
			Name = name;
		}

		protected override string Name { get; }

		protected override Type DetermineFromMapping( IServiceRegistry registry, Type subject ) => @as ?? subject.Adapt().GetConventionCandidate();
	}

	public class RegisterByConventionType : ConventionRegistration
	{
		public static RegisterByConventionType Instance { get; } = new RegisterByConventionType();

		RegisterByConventionType()
		{}

		protected override Type DetermineFromMapping( IServiceRegistry registry, Type subject ) => subject;
	}

	public abstract class ConventionRegistration : IConventionRegistration
	{
		public void Register( IServiceRegistry registry, Type subject )
		{
			var from = DetermineFromMapping( registry, subject );
			registry.Register( from, subject, Name );
		}

		protected abstract Type DetermineFromMapping( IServiceRegistry registry, Type subject );

		protected virtual string Name => null;
	}
}