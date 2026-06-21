using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Query;

public interface IYield<in TIn, out TOut> : ISelect<TIn, IEnumerable<TOut>>;