using DragonSpark.Model.Commands;
using SendGrid;
using System;

namespace DragonSpark.SendGrid;

sealed class Register : ICommand<SendGridClientOptions>
{
	readonly Func<SendGridSettings> _settings;

	public Register(Func<SendGridSettings> settings) => _settings = settings;

	public void Execute(SendGridClientOptions parameter)
	{
		var settings = _settings();
		parameter.ApiKey = settings.ApiKey;
	}
}