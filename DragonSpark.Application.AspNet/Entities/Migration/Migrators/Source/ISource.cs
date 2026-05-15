using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

public interface ISource<T> : ISelect<Stop<SourceInput<T>>, IQueryable<T>>;