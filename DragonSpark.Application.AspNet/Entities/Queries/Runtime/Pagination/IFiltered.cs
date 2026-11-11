using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public interface IFiltered : ICondition<PageInput>;