using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Query;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Model.Sequences.Collections.Groups;

public class GroupCollection<T> : Select<GroupName, IList<T>>, IGroupCollection<T>
{
	public GroupCollection(IEnumerable<IGroup<T>> groups) : this(groups, GroupPairs<T>.Default) {}

	public GroupCollection(IEnumerable<IGroup<T>> groups, IGroupPairs<T> pairs)
		: this(groups.Select(pairs.ToDelegate()).ToOrderedDictionary()) {}

	public GroupCollection(IOrderedDictionary<GroupName, IList<T>> store)
		: this(store,
		       store.Select(x => x.Value.ToArray())
		            .Select(SortAlteration<T>.Default.ToDelegate())
		            .SelectMany(x => x)
		            .To(Start.An.Extent<DeferredArray<T>>())) {}

	public GroupCollection(IOrderedDictionary<GroupName, IList<T>> store, IArray<T> values)
		: base(store.GetValue) => Values = values;

	public IArray<T> Values { get; }
}