using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct KeyRecord(string Signature, ImmutableArray<PropertyRecord> Properties)
{
	public KeyRecord(ImmutableArray<PropertyRecord> Properties)
		: this(string.Join("|", Properties.Select(p => p.Name)), Properties) {}

	public bool Equals(KeyRecord other)
		=> Properties.SequenceEqual(other.Properties, PropertyRecordEqualityComparer.Default);

	public override int GetHashCode()
	{
		var hash = new HashCode();
		foreach (var p in Properties)
			hash.Add(p);
		return hash.ToHashCode();
	}
}