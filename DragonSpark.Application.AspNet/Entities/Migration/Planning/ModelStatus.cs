using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public class ModelStatus : ISelect<DestinationModelCheckerInput, ModelStatusResult>
{
	readonly IReadOnlyDictionary<Type, Type> _forwarded;
	readonly ICondition<IsExactInput>        _exact;

	protected ModelStatus(params ForwardedType[] forwarded)
		: this(forwarded.ToDictionary(x => x.Previous, x => x.Current), IsExact.Default) {}

	public ModelStatus(IReadOnlyDictionary<Type, Type> forwarded, ICondition<IsExactInput> exact)
	{
		_forwarded = forwarded;
		_forwarded = forwarded;
		_exact     = exact;
	}

	public ModelStatusResult Get(DestinationModelCheckerInput parameter)
	{
		var (types, destination) = parameter;

		var locate    = new LocateType(destination, destination.GetEntityTypes().ToDictionary(t => t.Name), _forwarded);
		var exact     = new List<IEntityType>();
		var differing = new List<IEntityType>();
		var missing   = new List<IEntityType>();

		foreach (var from in types)
		{
			var to         = locate.Get(from);
			var collection = to is not null ? _exact.Get(new(from, to)) ? exact : differing : missing;
			collection.Add(from);
		}

		return new(new(exact.AsReadOnly(), differing.AsReadOnly()), missing.AsReadOnly());
	}
}

sealed class LocateType : ISelect<IEntityType, IEntityType?>
{
	readonly IModel                                   _destination;
	readonly IReadOnlyDictionary<string, IEntityType> _named;
	readonly IReadOnlyDictionary<Type, Type>          _forwarded;

	public LocateType(IModel destination, IReadOnlyDictionary<string, IEntityType> named,
	                  IReadOnlyDictionary<Type, Type> forwarded)
	{
		_forwarded   = forwarded;
		_named       = named;
		_destination = destination;
	}

	public IEntityType? Get(IEntityType parameter) => _named.TryGetValue(parameter.Name, out var to)
		                                                  ? to
		                                                  : _forwarded.TryGetValue(parameter.ClrType, out var forwarded)
			                                                  ? _destination.FindEntityType(forwarded)
			                                                  : null;
}

public readonly record struct ForwardedType(Type Previous, Type Current);