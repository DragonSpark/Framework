using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities.Configure;

sealed class AppendedStorageConfiguration : IStorageConfiguration
{
	readonly IStorageConfiguration _previous, _append;

	public AppendedStorageConfiguration(IStorageConfiguration previous, IStorageConfiguration append)
	{
		_previous = previous;
		_append   = append;
	}

	public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
		=> Start.A.Command<DbContextOptionsBuilder>()
		        .By.Calling(_previous.Get(parameter))
		        .Append(_append.Get(parameter));
}