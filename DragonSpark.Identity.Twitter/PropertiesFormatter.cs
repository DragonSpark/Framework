using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace DragonSpark.Identity.Twitter;

sealed class PropertiesFormatter : ISecureDataFormat<AuthenticationProperties>
{
	readonly IDistributedCache                           _store;
	readonly ISecureDataFormat<AuthenticationProperties> _previous;

	public PropertiesFormatter(IDistributedCache store, ISecureDataFormat<AuthenticationProperties> previous)
	{
		_store    = store;
		_previous = previous;
	}

	public string Protect(AuthenticationProperties data) => Store(_previous.Protect(data));

	public string Protect(AuthenticationProperties data, string? purpose)
		=> Store(_previous.Protect(data, purpose));

	string Store(string parameter)
	{
		var result = Guid.NewGuid().ToString();
		_store.SetString(result, parameter);
		return result;
	}

	public AuthenticationProperties? Unprotect(string? protectedText)
	{
		if (protectedText is not null)
		{
			var content = _store.GetString(protectedText);
			if (content is not null)
			{
				return _previous.Unprotect(content);
			}
		}

		return null;
	}

	public AuthenticationProperties? Unprotect(string? protectedText, string? purpose)
	{
		if (protectedText is not null)
		{
			var content = _store.GetString(protectedText);
			if (content is not null)
			{
				return _previous.Unprotect(content, purpose);
			}
		}

		return null;
	}
}