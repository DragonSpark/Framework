using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct ComparisonResult(
	IEntityType From,
	IEntityType To,
	PropertyComparison Properties,
	NavigationComparison Navigations,
	uint Changes)
{
	// ReSharper disable once TooManyDependencies
	public ComparisonResult(IEntityType From, IEntityType To, PropertyComparison Properties,
	                        NavigationComparison Navigations)
		: this(From, To, Properties, Navigations, Properties.Changes + Navigations.Changes) {}
}