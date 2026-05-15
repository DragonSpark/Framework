namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record EntityModifications(
	KeyChanges Keys,
	PropertyChanges Properties,
	NavigationChanges Navigations,
	uint Changes)
{
	public EntityModifications(KeyChanges Keys, PropertyChanges Properties, NavigationChanges Navigations)
		: this(Keys, Properties, Navigations, Keys.Changes + Properties.Changes + Navigations.Changes) {}
}