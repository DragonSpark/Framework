﻿using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Requests;

public class Header : ISelect<IHeaderDictionary, string?>
{
	readonly string _name;

	protected Header(string name) => _name = name;

	public string? Get(IHeaderDictionary parameter)
		=> parameter.TryGetValue(_name, out var value) ? value.ToString() : null;
}