using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Presentation.Components.Content;

public interface IActiveContent<T> : IResulting<T?>, IOperation<Action>, IConditionAware {}