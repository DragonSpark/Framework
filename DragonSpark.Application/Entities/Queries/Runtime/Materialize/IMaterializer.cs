﻿using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public interface IMaterializer<T, TResult> : ISelecting<TokenAware<IQueryable<T>>, TResult>;