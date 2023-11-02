using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content;

public interface IActiveContent<T> : IResulting<T?>, ICommand, IConditionAware;