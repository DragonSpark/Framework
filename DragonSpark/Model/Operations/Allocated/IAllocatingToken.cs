﻿using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocatingToken<T, TOut> : ISelect<Token<T>, Task<TOut>>;