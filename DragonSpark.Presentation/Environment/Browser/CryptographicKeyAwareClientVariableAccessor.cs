﻿using DragonSpark.Model;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class CryptographicKeyAwareClientVariableAccessor<T> : IClientVariableAccessor<T>
{
	readonly IClientVariableAccessor<T> _previous;

	public CryptographicKeyAwareClientVariableAccessor(IClientVariableAccessor<T> previous) => _previous = previous;

	public async ValueTask<ProtectedBrowserStorageResult<T>> Get(string parameter)
	{
		try
		{
			return await _previous.Get(parameter);
		}
		catch (CryptographicException)
		{
			await Remove.Get(parameter);
			return new();
		}
	}

	public ValueTask Get(Pair<string, T> parameter) => _previous.Get(parameter);

	public IRemove Remove => _previous.Remove;
}