using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using System;
using System.Linq;

namespace DragonSpark.Setup
{
	public sealed class RegisterAsAttribute : RegistrationBaseAttribute
	{
		public RegisterAsAttribute( Type @as ) : this( @as, null )
		{}

		public RegisterAsAttribute( string name ) : this( null, name )
		{}

		public RegisterAsAttribute( Type @as, string name )
		{
			As = @as;
			Name = name;
		}

		public Type As { get; }

		public string Name { get; }

		protected override void OnRegistration( IServiceRegistry registry, Type subject )
		{
			var from = As ?? subject.Extend().GetAllInterfaces().First( type => subject.Name.Contains( type.Name.Substring( 1 ) ) );
			registry.Register( from, subject, Name );
		}
	}

	public class RegisterResultTypeAttribute : RegistrationBaseAttribute
	{
		protected override void OnRegistration( IServiceRegistry registry, Type subject )
		{
			// throw new NotImplementedException();
		}
	}

	[AttributeUsage( AttributeTargets.Class )]
	public abstract class RegistrationBaseAttribute : Attribute, IConventionRegistration
	{
		protected abstract void OnRegistration( IServiceRegistry registry, Type subject );

		public void Register( IServiceRegistry registry, Type subject )
		{
			OnRegistration( registry, subject );
		}
	}

	public interface IConventionRegistration
	{
		void Register( IServiceRegistry registry, Type subject );
	}
}