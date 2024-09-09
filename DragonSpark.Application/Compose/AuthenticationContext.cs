﻿using DragonSpark.Compose;
using DragonSpark.Compose.Model.Commands;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Application.Compose;

public sealed class AuthenticationContext : IResult<ApplicationProfileContext>
{
	readonly ApplicationProfileContext             _subject;
	readonly CommandComposer<AuthenticationBuilder> _configure;
	readonly System.Action<AuthenticationOptions>? _options;

	public AuthenticationContext(ApplicationProfileContext subject) : this(subject, null) {}

	public AuthenticationContext(ApplicationProfileContext subject,
	                             System.Action<AuthenticationOptions>? options = null)
		: this(subject, Start.A.Command<AuthenticationBuilder>().By.Empty, options) {}

	public AuthenticationContext(ApplicationProfileContext subject,
	                             System.Action<AuthenticationBuilder> builder,
	                             System.Action<AuthenticationOptions>? options = null)
		: this(subject, Start.A.Command(builder), options) {}

	public AuthenticationContext(ApplicationProfileContext subject,
	                             CommandComposer<AuthenticationBuilder> configure,
	                             System.Action<AuthenticationOptions>? options = null)
	{
		_subject   = subject;
		_configure = configure;
		_options   = options;
	}

	public AuthenticationContext Append(ICommand<AuthenticationBuilder> command) => Append(command.Execute);

	public AuthenticationContext Append(System.Action<AuthenticationBuilder> command)
		=> new(_subject, _configure.Append(command));

	public ApplicationProfileContext Then => Get();

	public ApplicationProfileContext Get() => _subject.Append(new AuthenticationContextCommand(_configure, _options));
}