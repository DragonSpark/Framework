using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public interface IWhere<T> : ISelect<IEntityType, Expression<Func<T, bool>>>;