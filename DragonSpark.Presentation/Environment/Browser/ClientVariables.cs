using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;

namespace DragonSpark.Presentation.Environment.Browser;

public class ClientVariables<T> : ISelect<string, IClientVariable<T>>
{
	readonly ProtectedBrowserStorage _storage;
	readonly Type                    _type;

	protected ClientVariables(ProtectedBrowserStorage storage, Type type)
	{
		_storage = storage;
		_type    = type;
	}

	public IClientVariable<T> Get(string parameter)
		=> new ClientVariable<T>(new ClientVariableKey(_type, parameter).ToString(), _storage);
}