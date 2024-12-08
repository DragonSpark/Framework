using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public interface ISession<in TIn, TOut, in TSave> : ISelecting<TIn, TOut?>, IOperation<TSave>;