using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
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
}