using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct DestinationModelCheckerInput(IReadOnlyCollection<IEntityType> Types, IModel Destination);