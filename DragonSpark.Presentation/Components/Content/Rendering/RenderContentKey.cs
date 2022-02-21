﻿using DragonSpark.Application.Components;
using DragonSpark.Application.Security.Identity;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderContentKey : IRenderContentKey
{
	readonly IClientIdentifier     _identifier;
	readonly ICurrentUserName      _name;
	readonly ContentIdentification _content;

	public RenderContentKey(IClientIdentifier identifier, ICurrentUserName name, ContentIdentification content)
	{
		_identifier = identifier;
		_name       = name;
		_content    = content;
	}

	public string Get(object parameter) => $"{_identifier.Get().ToString()}/{_name.Get()}/{_content.Get(parameter)}";
}