using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class KeyRecordEqualityComparer : IEqualityComparer<KeyRecord>
{
	public static KeyRecordEqualityComparer Default { get; } = new();

	readonly IEqualityComparer<PropertyRecord> _property;

	public KeyRecordEqualityComparer()
		: this(PropertyRecordEqualityComparer.Default) {}

	public KeyRecordEqualityComparer(IEqualityComparer<PropertyRecord> property)
		=> _property = property;

	public bool Equals(KeyRecord x, KeyRecord y)
		=> x.Properties.SequenceEqual(y.Properties, _property);

	public int GetHashCode(KeyRecord obj)
	{
		var hash = new HashCode();
		foreach (var p in obj.Properties)
			hash.Add(p, _property);
		return hash.ToHashCode();
	}
}