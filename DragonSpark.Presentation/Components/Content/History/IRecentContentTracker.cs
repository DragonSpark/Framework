using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.Content.History;

public interface IRecentContentTracker<T> : IPopAware<ComponentKeyInput, ExistingContentResult<T>>,
                                            IAssign<ComponentEntryKey, T?>;