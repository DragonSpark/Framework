using System.IO;

namespace DragonSpark.Serialization
{
	public interface IStreamResolver
	{
		Stream ResolveStream();
	}
}