using System;
using DragonSpark.Testing.Framework.Setup.Location;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AuthorizedLocationSpecification : CanLocateSpecification
	{
		readonly IServiceLocationAuthority authorized;

		public AuthorizedLocationSpecification( IServiceLocator locator, IServiceLocationAuthority authorized ) : base( locator )
		{
			this.authorized = authorized;
		}

		protected override bool CanLocate( Type type )
		{
			return authorized.IsAllowed(type) && base.CanLocate( type );
		}
	}
}