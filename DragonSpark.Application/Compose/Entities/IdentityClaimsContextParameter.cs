using Microsoft.AspNetCore.Identity;
using System;

namespace DragonSpark.Application.Compose.Entities {
	public struct IdentityClaimsContextParameter<T>
	{
		public IdentityClaimsContextParameter(ApplicationProfileContext context, Action<IdentityOptions> configure,
		                                      Func<ExternalLoginInfo, T> create)
		{
			Context   = context;
			Configure = configure;
			Create    = create;
		}

		public ApplicationProfileContext Context { get; }

		public Action<IdentityOptions> Configure { get; }

		public Func<ExternalLoginInfo, T> Create { get; }
	}
}