using System;

namespace DragonSpark.Server.Requests;

public readonly record struct Unique<T>(string UserName, Guid Identity, T Subject);

public readonly record struct Unique(string UserName, Guid Identity);