using System;
using DragonSpark.Testing.Framework.Setup.Location;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AuthorizedLocationSpecification : CanLocateSpecification
	{
		readonly IServiceLocationAuthority authorized;

		public AuthorizedLocationSpecification( IServiceLocator locator, [Required]IServiceLocationAuthority authorized ) : base( locator )
		{
			this.authorized = authorized;
		}

		protected override bool CanLocate( Type type ) => authorized.IsAllowed( type ) && base.CanLocate( type );
	}
}