using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Composition;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using IdentityUser = DragonSpark.Application.AspNet.Security.Identity.IdentityUser;

namespace DragonSpark.Application.AspNet.Compose.Entities;

public sealed class IdentityStorageType<T, TContext> where TContext : DbContext where T : IdentityUser
{
	readonly ApplicationProfileContext _subject;
	readonly Action<IdentityOptions>   _configure;

	public IdentityStorageType(ApplicationProfileContext subject, Action<IdentityOptions> configure)
	{
		_subject   = subject;
		_configure = configure;
	}

	public IdentityStorageUsing<T, TContext> Application()
		=> new(_subject.Append(ApplicationRegistrations<TContext, T>.Default)
		               .Configure(x => x.ComposeUsing(Security.Identity.Compose.Default)
		                                .ComposeUsing(Security.Identity.Authentication.Compose.Default))
		               .Append(AddIdentityComponents<T>.Default)
		               .Append(Registrations<TContext>.Default)
		               .Append(Registrations.Default)
		               .Configure(x => x.ComposeUsing(AspNet.Entities.Queries.Runtime.Pagination.Compose
		                                                    .Default)),
		       _configure);

	public IdentityStorageUsing<T, TContext> Is
		=> new(_subject.Append(CommonRegistrations<TContext, T>.Default)
		               .Configure(x => x.ComposeUsing(Security.Identity.Compose.Default)
		                                .ComposeUsing(Security.Identity.Authentication.Compose.Default))
		               .Append(Registrations<TContext>.Default)
		               .Append(Registrations.Default),
		       _configure);
}