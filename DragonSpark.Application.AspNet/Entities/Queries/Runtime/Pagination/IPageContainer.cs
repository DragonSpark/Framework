using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public interface IPageContainer<T> : IOperation<PageResult<T>>,
                                     IOperation<Exception>,
                                     IAlteration<IPages<T>>,
                                     IReportedTypeAware;