using DragonSpark.Activation;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Setup.Registration
{
	public class TypeRegistration : IRegistration
	{
		readonly Type @as;
		readonly Type type;
		readonly string name;

		public TypeRegistration( Type type, string name = null ) : this( type, type, name ) {}

		public TypeRegistration( [Required]Type @as, [Required]Type type, string name = null )
		{
			this.@as = @as;
			this.type = type;
			this.name = name;
		}

		public void Register( IServiceRegistry registry ) => registry.Register( new MappingRegistrationParameter( @as, type, name ) );
	}
}