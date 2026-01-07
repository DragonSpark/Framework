using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class DefaultBatchSize : Instance<ushort>
{
	public static DefaultBatchSize Default { get; } = new();

	DefaultBatchSize() : base(5_000) {}
}

public readonly record struct BatchesInput(ILogger Logger, ushort BatchSize)
{
	public BatchesInput(ILogger logger) : this(logger, DefaultBatchSize.Default) {}
}

public readonly record struct MapInput(DbContext Source, DbContext Destination, object From, object To);

public interface IMap : ICommand<MapInput>;

public sealed class Map : IMap
{
	public static Map Default { get; } = new();

	Map() : this(EnumerationAwareProperty.Default) {}

	readonly IProperty _property;

	public Map(IProperty property) => _property = property;

	public void Execute(MapInput parameter)
	{
		var (source, destination, from, to) = parameter;
		var type = to.GetType();
		foreach (var propertyInfo in from.GetType().GetProperties())
		{
			var property = type.GetProperty(propertyInfo.Name);
			if (property is not null)
			{
				_property.Execute(new(source, destination, new(from, propertyInfo), new(to, property)));
			}
		}
	}
}

public readonly record struct InstanceInput(object Instance, PropertyInfo Metadata);

public readonly record struct PropertyInput(
	DbContext Source,
	DbContext Destination,
	InstanceInput From,
	InstanceInput To);

public interface IProperty : ICommand<PropertyInput>;

sealed class EnumerationAwareProperty : IProperty
{
	public static EnumerationAwareProperty Default { get; } = new();

	EnumerationAwareProperty() : this(Property.Default, ConvertEnumeration.Default) {}

	readonly IProperty                                                                  _previous;
	readonly DragonSpark.Model.Selection.Conditions.ICondition<ConvertEnumerationInput> _convert;

	public EnumerationAwareProperty(IProperty previous,
	                                DragonSpark.Model.Selection.Conditions.ICondition<ConvertEnumerationInput> convert)
	{
		_previous = previous;
		_convert  = convert;
	}

	public void Execute(PropertyInput parameter)
	{
		var (_, _, from, to) = parameter;
		if (!_convert.Get(new(from, to)))
		{
			_previous.Execute(parameter);
		}
	}
}

public readonly record struct ConvertEnumerationInput(InstanceInput From, InstanceInput To);

sealed class ConvertEnumeration : DragonSpark.Model.Selection.Conditions.ICondition<ConvertEnumerationInput>
{
	public static ConvertEnumeration Default { get; } = new();

	ConvertEnumeration() : this(PropertyDelegates.Default, PropertyAssignmentDelegates.Default) {}

	readonly IPropertyDelegates          _get;
	readonly IPropertyAssignmentDelegate _assign;

	public ConvertEnumeration(IPropertyDelegates get, IPropertyAssignmentDelegate assign)
	{
		_get    = get;
		_assign = assign;
	}

	public bool Get(ConvertEnumerationInput parameter)
	{
		var (from, to) = parameter;
		if (from.Metadata.PropertyType.IsEnum && to.Metadata.PropertyType.IsEnum)
		{
			var previous = Enum.GetUnderlyingType(from.Metadata.PropertyType);
			var next     = Enum.GetUnderlyingType(to.Metadata.PropertyType);
			var result   = TypeDescriptor.GetConverter(previous).CanConvertTo(next) && Convert(from, to, next);
			return result;
		}

		return false;
	}

	bool Convert(InstanceInput from, InstanceInput to, Type underlying)
	{
		var get = _get.Get(new(from.Metadata.ReflectedType ?? from.GetType(), from.Metadata.Name));
		if (get is not null)
		{
			var value   = get(from.Instance);
			var changed = System.Convert.ChangeType(value, underlying);
			if (changed is not null)
			{
				var converted = Enum.ToObject(to.Metadata.PropertyType, changed);
				_assign.Get(to.Metadata)(to.Instance, converted);
				return true;
			}
		}

		return false;
	}
}

sealed class Property : IProperty
{
	public static Property Default { get; } = new();

	Property() : this(PropertyDelegates.Default, PropertyAssignmentDelegates.Default) {}

	readonly IPropertyDelegates          _get;
	readonly IPropertyAssignmentDelegate _assign;

	public Property(IPropertyDelegates get, IPropertyAssignmentDelegate assign)
	{
		_get    = get;
		_assign = assign;
	}

	public void Execute(PropertyInput parameter)
	{
		var (_, _, from, to) = parameter;

		if (to.Metadata.PropertyType.IsAssignableFrom(from.Metadata.PropertyType))
		{
			var get = _get.Get(new(from.Metadata.ReflectedType ?? from.GetType(), from.Metadata.Name));
			if (get is not null)
			{
				_assign.Get(to.Metadata)(to.Instance, get(from.Instance));
			}
		}
	}
}

public readonly record struct MappingInput<T>(DbContext Source, DbContext Destination, T From);

public interface IMapping<TFrom, out TTo> : ISelect<MappingInput<TFrom>, TTo>;

public sealed class Mapping<TFrom, TTo> : IMapping<TFrom, TTo> where TFrom : class where TTo : class
{
	public static Mapping<TFrom, TTo> Default { get; } = new();

	Mapping() : this(Map.Default) {}

	readonly Func<TTo> _new;
	readonly IMap      _map;

	public Mapping(IMap map) : this(A.New<TTo>, map) {}

	public Mapping(Func<TTo> @new, IMap map)
	{
		_new = @new;
		_map = map;
	}

	public TTo Get(MappingInput<TFrom> parameter)
	{
		var (source, destination, from) = parameter;
		var result = _new();
		_map.Execute(new(source, destination, from, result));
		return result;
	}
}

public sealed record Batching<T>(DbContext Source, DbContext Destination, IQueryable<T> Subject);

public class Migration<T> : Migration
{
	protected Migration(params IBatches[] batches) : this(DefaultLog<T>.Default.Get(), batches) {}

	protected Migration(ILogger logger, params IBatches[] batches) : base(logger, batches) {}
}

public class Migration : ICommand<ushort>, ICommand
{
	readonly ILogger         _logger;
	readonly Array<IBatches> _batches;

	protected Migration(ILogger logger, params IBatches[] batches)
	{
		_logger  = logger;
		_batches = batches;
	}

	public void Execute(ushort parameter)
	{
		var input = new BatchesInput(_logger, parameter);
		foreach (var batch in _batches)
		{
			batch.Execute(input);
		}
	}

	public void Execute(None parameter)
	{
		Execute(DefaultBatchSize.Default);
	}
}

public interface IBatches : ICommand<BatchesInput>;

public class Batches<TFrom, TTo> : IBatches where TFrom : class where TTo : class
{
	readonly Batching<TFrom>    _batching;
	readonly IBatch<TFrom, TTo> _batch;

	protected Batches(DbContext Source, DbContext Destination, IQueryable<TFrom> Subject)
		: this(new(Source, Destination, Subject)) {}

	protected Batches(Batching<TFrom> batching) : this(batching, Batch<TFrom, TTo>.Default) {}

	protected Batches(Batching<TFrom> batching, IBatch<TFrom, TTo> batch)
	{
		_batching = batching;
		_batch    = batch;
	}

	public void Execute(BatchesInput parameter)
	{
		var (logger, size)                 = parameter;
		var (source, destination, subject) = _batching;
		var to    = destination.Set<TTo>();
		var total = subject.Count().Grade();
		for (var offset = 0; offset < total; offset += size)
		{
			_batch.Execute(new(logger, source, destination, subject, to, new(offset, size), total));
		}
	}
}

public sealed record BatchInput<TFrom, TTo>(
	ILogger Logger,
	DbContext Source,
	DbContext Destination,
	IQueryable<TFrom> From,
	DbSet<TTo> To,
	Partition Partition,
	uint Total) where TTo : class;

public interface IBatch<TFrom, TTo> : ICommand<BatchInput<TFrom, TTo>> where TTo : class;

public sealed class Batch<TFrom, TTo> : IBatch<TFrom, TTo> where TFrom : class where TTo : class
{
	public static Batch<TFrom, TTo> Default { get; } = new();

	Batch() : this(Mapping<TFrom, TTo>.Default) {}

	readonly IMapping<TFrom, TTo> _map;

	public Batch(IMapping<TFrom, TTo> map) => _map = map;

	public void Execute(BatchInput<TFrom, TTo> parameter)
	{
		var (logger, source, destination, from, to, (skip, top), total) = parameter;
		var watch  = Stopwatch.StartNew();
		var offset = skip.Value();
		using var batch = from.AsValueEnumerable()
		                      .Skip(offset)
		                      .Take(top.Value())
		                      .Select(x => _map.Get(new(source, destination, x)))
		                      .ToArray(ArrayPool<TTo>.Shared);

		to.AddRange(batch);

		var count = destination.SaveChanges();

		logger.LogInformation("{From} -> {To}: Batch of {Count} processed in {Elapsed:mm\\:ss\\.fff} ({Rate:F1} entities/sec)",
		                      A.Type<TFrom>(), A.Type<TTo>(), count, watch.Elapsed,
		                      count / watch.Elapsed.TotalSeconds);

		logger.LogDebug("Progress: {Processed}/{Total} ({Percent:F1}%)",
		                offset + count, total, (offset + count) / (double)total * 100);
	}
}