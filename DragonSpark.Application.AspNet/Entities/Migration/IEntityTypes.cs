using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public interface IEntityTypes : ISelect<IEntityType, IEntityType?>, IResult<IModel>;