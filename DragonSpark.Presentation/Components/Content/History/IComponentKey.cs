using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content.History;

public interface IComponentKey : ISelect<ComponentKeyInput, ComponentKeyHash>,
                                 ISelect<ComponentEntryKey, int>,
                                 ICommand<ComponentEntryKey>;