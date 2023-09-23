using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using System;

namespace DragonSpark.Application.Communication;

public sealed class ApplySameSiteCookiePolicy : ICommand<CookiePolicyOptions>
{
	public static ApplySameSiteCookiePolicy Default { get; } = new();

	ApplySameSiteCookiePolicy() : this(AssignSameSite.Default.Execute, AssignSameSite.Default.Execute) {}

	readonly Action<AppendCookieContext> _configure;
	readonly Action<DeleteCookieContext> _delete;

	public ApplySameSiteCookiePolicy(Action<AppendCookieContext> configure, Action<DeleteCookieContext> delete)
	{
		_configure = configure;
		_delete    = delete;
	}

	public void Execute(CookiePolicyOptions parameter)
	{
		parameter.OnAppendCookie = _configure;
		parameter.OnDeleteCookie = _delete;
	}
}