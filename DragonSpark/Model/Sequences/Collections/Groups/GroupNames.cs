using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Groups;

public class GroupNames : TableValues<string, GroupName>
{
	public GroupNames(params GroupName[] names) : this(names.ToOrderedDictionary(x => x.Name)) {}

	public GroupNames(IDictionary<string, GroupName> store) : base(store) {}
}