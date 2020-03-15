using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class IdentityContext
	{
		readonly ApplicationProfileContext _context;
		readonly Action<IdentityOptions>   _configure;

		public IdentityContext(ApplicationProfileContext context, Action<IdentityOptions> configure)
		{
			_context   = context;
			_configure = configure;
		}

		public IdentityClaimsContext<T> CreatedWith<T>(ISelect<ExternalLoginInfo, T> create) where T : IdentityUser
			=> CreatedWith(create.Get);

		public IdentityClaimsContext<T> CreatedWith<T>(Func<ExternalLoginInfo, T> create) where T : IdentityUser
			=> new IdentityClaimsContext<T>(new IdentityClaimsContextParameter<T>(_context, _configure, create));
	}

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

	public sealed class IdentityClaimsContext<T> where T : IdentityUser
	{
		readonly IdentityClaimsContextParameter<T> _context;
		readonly IAppliedPrincipal                 _principal;

		public IdentityClaimsContext(IdentityClaimsContextParameter<T> context)
			: this(context, DefaultAppliedPrincipal.Default) {}

		public IdentityClaimsContext(IdentityClaimsContextParameter<T> context, IAppliedPrincipal principal)
		{
			_context   = context;
			_principal = principal;
		}

		public IdentityClaimsContext<T> Using(IAppliedPrincipal principal)
			=> new IdentityClaimsContext<T>(_context, principal);

		public ConfiguredIdentityContext<T> Having(IClaims claims)
			=> new ConfiguredIdentityContext<T>(_context.Context.Then(new IdentityRegistration<T>(_principal, claims,
			                                                                                      _context.Create)),
			                                    _context.Configure);
	}

	public sealed class ConfiguredIdentityContext<T> where T : IdentityUser
	{
		readonly ApplicationProfileContext _context;
		readonly Action<IdentityOptions>   _configure;

		public ConfiguredIdentityContext(ApplicationProfileContext context, Action<IdentityOptions> configure)
		{
			_context   = context;
			_configure = configure;
		}

		public EntityStorageContext<TContext, T> StoredIn<TContext>() where TContext : IdentityDbContext<T>
			=> new EntityStorageContext<TContext, T>(_context, _configure);
	}
}