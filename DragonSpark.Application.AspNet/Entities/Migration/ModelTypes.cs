using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public class ModelTypes : ReferenceValueStore<IModel, IEntityTypes>, IModelTypes
{
	protected ModelTypes(params ReadOnlySpan<ForwardedType> forwarded)
		: this(forwarded.AsValueEnumerable().ToDictionary(x => x.Previous, x => x.Current)) {}

	protected ModelTypes(IReadOnlyDictionary<Type, Type> forwarded) : base(x => new EntityTypes(x, forwarded)) {}
}