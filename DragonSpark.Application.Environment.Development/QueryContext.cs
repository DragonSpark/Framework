using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Environment.Development;

public readonly record struct QueryContext(Type Type, QueryTrackingBehavior Behavior);