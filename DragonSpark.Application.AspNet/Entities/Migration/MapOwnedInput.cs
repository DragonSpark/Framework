using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct MapOwnedInput(
	object? SourceValue, 
	NavigationEntry DestinationNavigation, 
	INavigation SourceNavigation
);