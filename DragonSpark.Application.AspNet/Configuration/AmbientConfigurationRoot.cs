using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Environment;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace DragonSpark.Application.Configuration;

sealed class AmbientConfigurationRoot : ISelect<IHostEnvironment, DirectoryInfo>
{
	public static AmbientConfigurationRoot Default { get; } = new();

	AmbientConfigurationRoot() : this(".configuration", PrimaryDirectory.Default) {}

	readonly string                 _name;
	readonly IResult<DirectoryInfo> _default;

	public AmbientConfigurationRoot(string name, IResult<DirectoryInfo> @default)
	{
		_name    = name;
		_default = @default;
	}

	public DirectoryInfo Get(IHostEnvironment parameter)
	{
		var path   = Path.Combine(parameter.ContentRootPath, _name);
		var result = Directory.Exists(path) ? new DirectoryInfo(path) : _default.Get();
		return result;
	}
}