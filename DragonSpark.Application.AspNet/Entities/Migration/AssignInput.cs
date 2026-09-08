using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct AssignInput(PropertyValues From, PropertyValues To);