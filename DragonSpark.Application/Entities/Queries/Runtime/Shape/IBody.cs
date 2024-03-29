﻿using DragonSpark.Model.Operations.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public interface IBody<T> : ISelecting<ComposeInput<T>, IQueryable<T>>;