using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;

namespace DragonSpark.Presentation.Components.Navigation;

sealed class QueryStringProperties : Properties<QueryStringParameterAttribute, QueryStringProperty>
{
	public static QueryStringProperties Default { get; } = new();

	QueryStringProperties() : base(Selector.Instance.Get) {}

	sealed class Selector : ISelect<Property<QueryStringParameterAttribute>, QueryStringProperty>
	{
		public static Selector Instance { get; } = new();

		Selector() {}

		public QueryStringProperty Get(Property<QueryStringParameterAttribute> parameter)
		{
			var (metadata, attribute) = parameter;
			var name = attribute.Name ?? metadata.Name;
			return new QueryStringProperty(metadata, name);
		}
	}
}