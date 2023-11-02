using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public interface IPaginationComposer<T> : IAlteration<IResulting<IPages<T>>>;