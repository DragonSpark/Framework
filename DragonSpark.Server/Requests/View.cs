using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonSpark.Server.Requests;

public readonly record struct View(Controller Owner, Guid Subject)
{
	public static implicit operator Query(View instance) => new (instance.Owner, instance.Subject);
}

public readonly record struct View<T>(Controller Owner, T Subject);