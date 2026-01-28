using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IEntities<TFrom, out TTo> : ISelect<Stop<ProcessChangesInput<TFrom>>, IQueryable<TTo>>;