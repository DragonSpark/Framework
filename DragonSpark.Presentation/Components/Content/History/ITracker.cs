using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content.History;

public interface ITracker<TIn, TOut> : ISelect<TIn, TOut?>, IAssign<TIn, TOut?>;