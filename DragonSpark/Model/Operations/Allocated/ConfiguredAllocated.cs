using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

[UsedImplicitly]
public class ConfiguredAllocated : IAllocated
{
	readonly Action _subject;

	public ConfiguredAllocated(Action configure) => _subject = configure;

	public Task Get()
	{
		_subject();
		return Task.CompletedTask;
	}
}