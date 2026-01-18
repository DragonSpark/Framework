using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct MappingInput(DbContext Source, DbContext Destination, object From, Type To);