using DragonSpark.Application.Components;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Presentation.Connections.Initialization;

sealed class InitializeConnection : IInitializeConnection
{
	readonly string            _name;
	readonly IClientIdentifier _identifier;

	public InitializeConnection(IClientIdentifier identifier) : this(ConnectionIdentifierName.Default, identifier) {}

	public InitializeConnection(string name, IClientIdentifier identifier)
	{
		_name       = name;
		_identifier = identifier;
	}

	public void Execute(HttpContext parameter)
	{
		var options = new CookieOptions
		{
			Expires     = DateTime.Now.AddMonths(1),
			IsEssential = true
		};

		parameter.Response.Cookies.Append(_name, _identifier.Get().ToString(), options);
	}
}