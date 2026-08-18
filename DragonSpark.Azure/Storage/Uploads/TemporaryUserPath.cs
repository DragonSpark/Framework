using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class TemporaryUserPath : IFormatter<UserInput<string>>
{
	readonly TemporaryUserRoot _root;

	public TemporaryUserPath(TemporaryUserRoot root) => _root = root;
	
	public string Get(UserInput<string> parameter)
	{
		var (user, path) = parameter;
		return $"{_root.Get(user.Contract())}/{path}";
	}
}