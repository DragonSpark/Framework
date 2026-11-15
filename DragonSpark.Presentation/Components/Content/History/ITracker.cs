using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.Content.History;

public interface ITracker<TIn, TOut> : IPopAware<TIn, TOut?>, IAssign<TIn, TOut?>;