﻿using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.Communication;

public class Header : IHeader
{
	readonly string _name;

	protected Header(string name) => _name = name;

	public string? Get(IHeaderDictionary parameter)
		=> parameter.TryGetValue(_name, out var value) ? value.ToString() : null;
}