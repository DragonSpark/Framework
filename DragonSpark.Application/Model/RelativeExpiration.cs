using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Model;

public class RelativeExpiration : ICommand<ICacheEntry>
{
	readonly TimeSpan _valid;

	public RelativeExpiration(TimeSpan valid) => _valid = valid;

	public void Execute(ICacheEntry parameter)
	{
		parameter.SetAbsoluteExpiration(_valid);
	}
}