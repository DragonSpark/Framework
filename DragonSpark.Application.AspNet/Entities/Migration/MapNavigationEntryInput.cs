using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct MapNavigationEntryInput(NavigationEntry From, NavigationEntry To);