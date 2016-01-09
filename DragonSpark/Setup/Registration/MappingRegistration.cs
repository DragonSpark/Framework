using DragonSpark.Activation;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Setup.Registration
{
	public class TypeRegistration : IRegistration
	{
		public static TypeRegistration Instance { get; } = new TypeRegistration();


		readonly Func<Type, Type> mapping;
		readonly Func<Type, Type> register;
		readonly string name;

		public TypeRegistration( string name = null ) : this( t => t, name ) {}

		protected TypeRegistration( [Required]Func<Type, Type> mapping, string name = null ) : this( mapping, type => type, name ) {}

		protected TypeRegistration( [Required]Func<Type, Type> mapping, [Required]Func<Type, Type> register, string name = null )
		{
			this.mapping = mapping;
			this.register = register;
			this.name = name;
		}

		public void Register( IServiceRegistry registry, Type subject ) => registry.Register( mapping( subject ), register( subject ), name );
	}

	public class MappingRegistration : TypeRegistration
	{
		public MappingRegistration( Type @as, string name ) : base( type => @as ?? type.Adapt().GetConventionCandidate(), name ) {}
	}
}