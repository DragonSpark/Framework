using System;
using System.Security.Principal;
using DragonSpark.Application.Presentation.Navigation;
using DragonSpark.Application.Presentation.Security;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Infrastructure
{
	public class SecurityValidator : IViewValidator
	{
		readonly ISecurityManager manager;
		readonly IPrincipal principal;
		
		public SecurityValidator( ISecurityManager manager, IPrincipal principal )
		{
			this.manager = manager;
			this.principal = principal;
		}

		public void Validate( ViewValidationContext context )
		{
			manager.Verify( principal, context.Location.ToString() ).IsFalse( () =>
			{
				throw new UnauthorizedAccessException( "You do not have adequate permissions to perform this operation." );
			} );
		}
	}
}