﻿using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class ComponentStateVariable<T> : ClientVariable<T>
{
	public ComponentStateVariable(string key, ProtectedBrowserStorage storage) : base(key, storage) {}
}