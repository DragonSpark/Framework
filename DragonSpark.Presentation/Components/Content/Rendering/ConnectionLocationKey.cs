using DragonSpark.Text;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ConnectionLocationKey : IFormatter<Guid>
{
	readonly NavigationManager _manager;

	public ConnectionLocationKey(NavigationManager manager) => _manager = manager;

	public string Get(Guid parameter) => $"{_manager.Uri}+{parameter.ToString()}";
}