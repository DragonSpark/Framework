using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public interface IPageContainer<T> : ICommand<Page<T>>;