﻿namespace DragonSpark.Application.Security.Identity.Model;

public sealed record LoginErrorRedirect : ErrorRedirect
{
	public LoginErrorRedirect(string message, string origin) : base("./Error", message, origin) {}
}