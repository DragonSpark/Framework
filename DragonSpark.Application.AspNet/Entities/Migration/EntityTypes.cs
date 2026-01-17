using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

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