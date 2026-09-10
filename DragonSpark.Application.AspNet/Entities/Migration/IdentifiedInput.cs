using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct IdentifiedInput(PropertyValues From, PropertyValues To);