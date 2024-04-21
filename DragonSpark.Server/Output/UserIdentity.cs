using DragonSpark.Application.Model;

namespace DragonSpark.Server.Output;

public readonly record struct UserIdentity(uint Identity) : IUserIdentity
{
	public uint Get() => Identity;
}