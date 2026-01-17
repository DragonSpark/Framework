using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

class Class3;

// TODO
public interface IModelTypes : ISelect<IModel, IEntityTypes>;

public class ModelTypes : ReferenceValueStore<IModel, IEntityTypes>, IModelTypes
{
	protected ModelTypes(params ReadOnlySpan<ForwardedType> forwarded)
		: this(forwarded.AsValueEnumerable().ToDictionary(x => x.Previous, x => x.Current)) {}

	protected ModelTypes(IReadOnlyDictionary<Type, Type> forwarded) : base(x => new EntityTypes(x, forwarded)) {}
}

public interface IEntityTypes : ISelect<IEntityType, IEntityType?>, IResult<IModel>;

sealed class EntityTypes : Instance<IModel>, IEntityTypes
{
	readonly IModel                                   _model;
	readonly IReadOnlyDictionary<string, IEntityType> _named;
	readonly IReadOnlyDictionary<Type, Type>          _forwarded;

	public EntityTypes(IModel model, IReadOnlyDictionary<Type, Type> forwarded)
		: this(model, model.GetEntityTypes().ToDictionary(t => t.Name), forwarded) {}

	public EntityTypes(IModel model, IReadOnlyDictionary<string, IEntityType> named,
	                   IReadOnlyDictionary<Type, Type> forwarded) : base(model)
	{
		_forwarded = forwarded;
		_named     = named;
		_model     = model;
	}

	public IEntityType? Get(IEntityType parameter) => _named.TryGetValue(parameter.Name, out var to)
		                                                  ? to
		                                                  : _forwarded.TryGetValue(parameter.ClrType, out var forwarded)
			                                                  ? _model.FindEntityType(forwarded)
			                                                  : null;
}

public readonly record struct ForwardedType(Type Previous, Type Current);