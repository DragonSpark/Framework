using System;

namespace DragonSpark.Server.Requests;

public readonly record struct Unique<T>(uint? User, Guid Identity, T Subject);

public readonly record struct Unique(uint? User, Guid Identity);