﻿using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Editing;

public interface IEdit<in TIn, T> : ISelecting<TIn, Edit<T>> {}