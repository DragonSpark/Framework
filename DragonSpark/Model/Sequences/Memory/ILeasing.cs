﻿using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Model.Sequences.Memory;

public interface ILeasing<in TIn, T> : ISelecting<TIn, Leasing<T>>;