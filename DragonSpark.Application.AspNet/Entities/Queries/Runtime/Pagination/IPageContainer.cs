using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public interface IPageContainer<T> : ICommand<Page<T>>, ICommand<Exception>, IAlteration<IPages<T>>, IReportedTypeAware;