using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;

namespace DragonSpark.Application.Security.Identity.State;

public abstract class ConfigureCookie<T> : ICommand<CookieAuthenticationOptions> where T : SystemServerSettings
{
	readonly Func<T> _settings;

	protected ConfigureCookie(Func<T> settings) => _settings = settings;

	public void Execute(CookieAuthenticationOptions parameter)
	{
		var settings = _settings();
		parameter.Cookie.Domain = settings.Domain;
	}
}