using DragonSpark.Model.Operations;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public interface IPartition<T> : ISelecting<Partition<T>, IQueryable<T>> {}