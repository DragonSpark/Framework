using DragonSpark.Model.Selection;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class CompareKeys : ISelect<CompareKeysInput, KeyChanges>
{
	public static CompareKeys Default { get; } = new();

	public CompareKeys() : this(PropertyRecordEqualityComparer.Default, EntityMetadataEqualityComparer.Default) {}

	readonly IEqualityComparer<PropertyRecord> _properties;
	readonly IEqualityComparer<Type>           _types;

	public CompareKeys(IEqualityComparer<PropertyRecord> properties, IEqualityComparer<Type> types)
	{
		_properties = properties;
		_types      = types;
	}

	public KeyChanges Get(CompareKeysInput parameter)
	{
		var (from, to) = parameter;

		// Added / Removed (structural)
		var added   = to.Set.Except(from.Set).ToImmutableArray();
		var removed = from.Set.Except(to.Set).ToImmutableArray();

		// Changed (signature match but structural mismatch)
		var changed = from.Set.Where(f => to.Map.ContainsKey(f.Signature))
		                  .Where(f =>
		                         {
			                         var t = to.Map[f.Signature];

			                         var fromProps = f.Properties;
			                         var toProps   = t.Properties;

			                         if (fromProps.Length != toProps.Length)
				                         return true;

			                         for (int i = 0; i < fromProps.Length; i++)
			                         {
				                         var fp = fromProps[i];
				                         var tp = toProps[i];
				                         if (!_properties.Equals(fp, tp) || !_types.Equals(fp.Type, tp.Type))
					                         return true;
			                         }

			                         return false;
		                         })
		                  .ToImmutableArray();

		return new(added, removed, changed);
	}
}