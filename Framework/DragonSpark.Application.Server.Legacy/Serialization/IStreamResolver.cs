using System.IO;

namespace DragonSpark.Server.Legacy.Serialization
{
	public interface IStreamResolver
	{
		Stream ResolveStream();
	}
}