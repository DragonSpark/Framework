using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Collections.Groups
{
	public readonly struct GroupName
	{
		public GroupName(string name) => Name = name;

		public string Name { get; }

		public bool Equals(GroupName other) => string.Equals(Name, other.Name);

		public override bool Equals(object obj)
			=> !ReferenceEquals(null, obj) && obj is GroupName phase && Equals(phase);

		public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;
	}

	sealed class GroupName<T> : Select<T, GroupName>, IGroupName<T>
	{
		public GroupName(GroupName defaultName, ISelect<string, GroupName> names)
			: base(Start.A.Selection<T>()
			            .By.Returning(defaultName)
			            .Unless(new MetadataGroupName<T>(names))) {}
	}
}