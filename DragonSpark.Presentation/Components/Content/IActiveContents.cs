using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content;

public interface IActiveContents<T> : ISelect<ActiveContentInput<T>, IActiveContent<T>> {}