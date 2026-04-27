using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Update<TFrom, TTo> : IEntities<TFrom, TTo>
	where TFrom : class
	where TTo   : class
{
	readonly IMap      _map;
	readonly Func<TTo> _activate;
	readonly Type      _from;

	public Update(IMap map) : this(map, A.New<TTo>, typeof(TFrom)) {}

	public Update(IMap map, Func<TTo> activate, Type from)
	{
		_map      = map;
		_activate = activate;
		_from     = from;
	}

	public IQueryable<TTo> Get(Stop<ProcessChangesInput<TFrom>> parameter)
	{
		var ((_, _, source, destination, from, _), stop) = parameter;

		using var names = source.Model.FindEntityType(_from)
								.Verify()
								.FindPrimaryKey()
								.Verify()
								.Properties.AsValueEnumerable()
								.Select(p => p.Name)
								.ToArray(ArrayPool<string>.Shared);

		
		
		// ReSharper disable AccessToDisposedClosure
		var projected = from.Select(x => new
		{
			Source = x,
			Keys   = names.Select(y => EF.Property<object>(x, y))
		});

		return new Entities<TTo>(EnumerateAsync());

		async IAsyncEnumerable<TTo> EnumerateAsync()
		{
			var memory = names.Memory;

			await foreach (var row in projected.AsAsyncEnumerable().WithCancellation(stop))
			{
				using var keys   = row.Keys.AsValueEnumerable().ToArray(ArrayPool<object>.Shared);
				var       item   = _activate();
				var       to     = destination.Entry(item);
				var       span   = memory.Span;
				var       values = keys.Memory.Span;

				for (var i = 0; i < names.Length; i++)
				{
					to.Property(span[i]).CurrentValue = values[i];
				}

				await _map.Off(new(new(source.Entry(row.Source), to), stop));

				yield return item;
			}
		}
	}
}

sealed class Update<T> : ISave<T> where T : class
{
	public static Update<T> Default { get; } = new();

	Update() {}

	public async ValueTask<uint> Get(Stop<SaveInput<T>> parameter)
	{
		var ((logger, size, destination, entities, total), stop) = parameter;
		var configuration = new BulkConfig { BatchSize = size, CalculateStats = true, NotifyAfter = size };
		await destination.BulkUpdateAsync(entities, configuration, new Progress<T>(logger, total).Execute,
										  cancellationToken: stop)
						 .Off();
		var result = configuration.StatsInfo.Verify().StatsNumberUpdated.Grade();
		return result;
	}
}