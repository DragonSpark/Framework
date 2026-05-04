using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Insert<T> : ISave<T> where T : class
{
	public static Insert<T> Default { get; } = new();

	Insert() : this(DefaultChunkFactor.Default) {}

	readonly byte _factor;

	public Insert(byte factor) => _factor = factor;

	public async ValueTask<uint> Get(Stop<SaveInput<T>> parameter)
	{
		var ((logger, size, destination, entities, total), stop) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize           = size,
			SqlBulkCopyOptions  = SqlBulkCopyOptions.KeepIdentity,
			PreserveInsertOrder = true, UseTempDB = false,
			NotifyAfter         = size, EnableShadowProperties = true, IncludeGraph = true
		};

		await foreach (var chunk in entities.AsAsyncEnumerable().Chunk(size * _factor).WithCancellation(stop))
		{
			using var page = chunk.AsValueEnumerable().ToArray(ArrayPool<T>.Shared);
			try
			{
				await destination.BulkInsertAsync(page, configuration, new Progress<T>(logger, total).Execute,
				                                  cancellationToken: stop)
				                 .Off();
			}
			catch (Exception e)
			{
				throw;
			}
		}

		return total;
	}
}