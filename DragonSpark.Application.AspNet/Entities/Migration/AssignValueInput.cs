using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct AssignValueInput(object Source, PropertyEntry Destination);