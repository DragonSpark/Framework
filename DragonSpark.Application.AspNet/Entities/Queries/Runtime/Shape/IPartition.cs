using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public interface IPartition<T> : ISelecting<Partition<T>, IQueryable<T>>;