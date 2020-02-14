﻿using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Application
{
	public static class Extensions
	{
		public static IdentityContext WithIdentity(this ApplicationProfileContext @this)
			=> @this.WithIdentity(options => {});

		public static IdentityContext WithIdentity(this ApplicationProfileContext @this,
		                                           Action<IdentityOptions> configure)
			=> new IdentityContext(@this, configure);


		public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
		                                                       params ICommand<AuthorizationOptions>[] policies)
			=> @this.AuthorizeUsing(policies.Select(x => x.ToDelegate()).ToArray());
		public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
		                                                       params Action<AuthorizationOptions>[] policies)
			=> @this.Then(new AuthorizeConfiguration(new ConfigurePolicies(policies).Execute));

		public static string UniqueId(this ExternalLoginInfo @this) => Security.Identity.UniqueId.Default.Get(@this);

		public static string Value(this ModelBindingContext @this, IResult<string> key)
			=> @this.ValueProvider.Get(key);

		public static string Get(this IValueProvider @this, IResult<string> key)
		{
			var name   = key.Get();
			var value  = @this.GetValue(name);
			var result = value != ValueProviderResult.None ? value.FirstValue : null;
			return result;
		}
	}
}