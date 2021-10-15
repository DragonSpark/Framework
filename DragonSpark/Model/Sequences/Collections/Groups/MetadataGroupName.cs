using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Collections.Groups;

class MetadataGroupName<T> : Select<T, GroupName>, IGroupName<T>
{
	public MetadataGroupName(ISelect<string, GroupName> names) : base(Start.An.Instance<DeclaredGroupNames<T>>()
	                                                                       .Select(names)
	                                                                       .Get) {}
}