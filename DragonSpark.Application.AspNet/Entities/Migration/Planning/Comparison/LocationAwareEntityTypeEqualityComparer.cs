using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class LocationAwareEntityTypeEqualityComparer : IEqualityComparer<IEntityType>
{
	readonly IEntityTypes                   _types;
	readonly IModel                         _model;
	readonly IEqualityComparer<IEntityType> _previous;

	public LocationAwareEntityTypeEqualityComparer(IEntityTypes types)
		: this(types, types.Get(), EntityTypeEqualityComparer.Default) {}

	public LocationAwareEntityTypeEqualityComparer(IEntityTypes types, IModel model,
	                                               IEqualityComparer<IEntityType> previous)
	{
		_types    = types;
		_model    = model;
		_previous = previous;
	}

	public bool Equals(IEntityType? x, IEntityType? y)
	{
		var first  = x is not null && x.Model != _model ? _types.Get(x) : x;
		var second = y is not null && y.Model != _model ? _types.Get(y) : y;
		return first is not null && second is not null && _previous.Equals(first, second);
	}

	public int GetHashCode(IEntityType obj)
	{
		var type = obj.Model != _model ? _types.Get(obj) ?? obj : obj;
		return _previous.GetHashCode(type);
	}
}