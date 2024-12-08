using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public interface IPageContainer<T> : ICommand<Page<T>>, ICommand<Exception>, IReportedTypeAware;