﻿using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public readonly record struct Composition<T>(IQueryable<T> Current, ulong? Count = null);