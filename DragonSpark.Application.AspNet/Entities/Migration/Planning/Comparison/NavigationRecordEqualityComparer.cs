using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class NavigationRecordEqualityComparer : IEqualityComparer<NavigationRecord>
{
	readonly IEqualityComparer<IEntityType> _type;

	public NavigationRecordEqualityComparer(IEqualityComparer<IEntityType> type) => _type = type;

	public bool Equals(NavigationRecord x, NavigationRecord y)
		=> x.Name == y.Name && _type.Equals(x.Type, y.Type) && x.IsCollection == y.IsCollection &&
		   x.IsOnDependent == y.IsOnDependent;

	public int GetHashCode(NavigationRecord obj)
		=> HashCode.Combine(obj.Name, _type.GetHashCode(obj.Type), obj.IsCollection, obj.IsOnDependent);
}