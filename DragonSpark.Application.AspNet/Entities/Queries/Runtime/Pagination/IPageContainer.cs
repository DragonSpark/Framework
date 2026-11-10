using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public interface IPageContainer<T> : IOperation<PageStatus<T>>,
                                     IOperation<Exception>,
                                     IAlteration<IPages<T>>,
                                     IReportedTypeAware;