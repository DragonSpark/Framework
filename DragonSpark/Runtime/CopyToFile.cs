using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Runtime;

public sealed class CopyToFile : ICopyToFile
{
	public static CopyToFile Default { get; } = new();

	CopyToFile() {}

	public async ValueTask Get(CopyInput parameter)
	{
		var (source, destination) = parameter;
		await using var result = File.OpenWrite(destination);
		await source.CopyToAsync(result).ConfigureAwait(false);
	}
}