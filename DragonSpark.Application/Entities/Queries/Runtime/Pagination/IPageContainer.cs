using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public interface IPageContainer<T> : ICommand<Page<T>>, ICommand<Exception>, IReportedTypeAware;