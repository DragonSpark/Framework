using System;

namespace DragonSpark.Application.Mobile.Security.Authentication;

public readonly record struct Session(string Identifier, Uri Address)
{
    public Session(Uri Address) : this(Guid.NewGuid().ToString(), Address) {}
}