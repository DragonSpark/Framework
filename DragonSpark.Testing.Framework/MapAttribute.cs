using DragonSpark.Setup.Registration;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Testing.Framework
{
	public class MapAttribute : RegistrationBaseAttribute
	{
		public MapAttribute( Type registrationType, Type mappedTo ) : base( () => new RegistrationCustomization( new MappingRegistration( registrationType, mappedTo ) ) ){}

		public class MappingRegistration : TypeRegistration
		{
			public MappingRegistration( [Required]Type registrationType, [Required]Type mappedTo ) : base( t => registrationType, t => mappedTo ) { }
		}
	}
}