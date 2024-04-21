using DragonSpark.Application.Model;
using DragonSpark.Text;

namespace DragonSpark.Server.Output;

public interface IUserOutputKey : IText
{
	string Get<T>(T parameter) where T : IUserIdentity;
}