using DragonSpark.Text;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class TemporaryUserRoot : IFormatter<uint>
{
	readonly string _root;

	public TemporaryUserRoot(TemporaryRoot settings) : this(settings.Get()) {}

	public TemporaryUserRoot(string root) => _root = root;
	
	public string Get(uint parameter) => $"{_root}/{parameter}";
}