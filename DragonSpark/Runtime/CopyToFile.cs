using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Runtime;

public sealed class CopyToFile : ICopyToFile
{
	public static CopyToFile Default { get; } = new();

	CopyToFile() {}

	public async ValueTask Get(Stop<CopyInput> parameter)
	{
		var ((source, destination), stop) = parameter;
		await using var result = File.OpenWrite(destination);
		await source.CopyToAsync(result, stop).Off();
	}
}