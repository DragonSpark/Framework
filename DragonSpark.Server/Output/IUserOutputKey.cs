using DragonSpark.Application.Model;

namespace DragonSpark.Server.Output;

public interface IUserOutputKey : IOutputKey
{
	string Get<T>(T parameter) where T : IUserIdentity;
}