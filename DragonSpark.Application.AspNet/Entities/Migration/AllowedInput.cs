using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct AllowedInput(IProperty Property, string Name, ImmutableList<IProperty> Properties);