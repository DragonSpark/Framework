using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct ModelStatusInput(Array<IEntityType> Types, IModel Destination);