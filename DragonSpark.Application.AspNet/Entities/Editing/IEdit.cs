﻿using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public interface IEdit<T> : IEdit<T, T>;

public interface IEdit<in TIn, T> : ISelecting<TIn, Edit<T>>;