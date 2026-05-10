using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class EntityTypes : ReferenceValueStore<IEntityType, IEntityType?>, IEntityTypes
{
	readonly IModel _model;

	public EntityTypes(IModel model, IReadOnlyDictionary<Type, Type> forwarded)
		: base(new ComposeEntityTypes(model, forwarded)) => _model = model;

	public IModel Get() => _model;
}