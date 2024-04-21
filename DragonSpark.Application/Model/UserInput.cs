using System;

namespace DragonSpark.Application.Model;

public readonly record struct UserInput<T>(ulong User, T Input) : IUserIdentity
{
	public uint Get() => (uint)User;
}

public readonly record struct UserInput(ulong User, Guid Input) : IUserIdentity
{
	public uint Get() => (uint)User;
}