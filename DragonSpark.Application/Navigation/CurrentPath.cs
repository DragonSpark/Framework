using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Application.Navigation;

public sealed class CurrentPath : IResult<string>
{
	readonly NavigationManager _manager;

	public CurrentPath(NavigationManager manager) => _manager = manager;

	public string Get() => new Uri(_manager.Uri).PathAndQuery;
}