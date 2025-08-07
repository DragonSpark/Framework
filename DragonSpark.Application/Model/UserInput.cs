using System;

namespace DragonSpark.Application.Model;

public readonly record struct UserInput<T>(ulong User, T Input) : IUserIdentity
{
	public static implicit operator uint(UserInput<T> instance) => instance.Get();
	public uint Get() => (uint)User;
}

public readonly record struct UserInput(ulong User, Guid Input) : IUserIdentity
{
	public static implicit operator uint(UserInput instance) => instance.Get();
	public uint Get() => (uint)User;
}