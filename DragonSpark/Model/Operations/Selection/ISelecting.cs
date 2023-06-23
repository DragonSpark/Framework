using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public interface ISelecting<in TIn, TOut> : ISelect<TIn, ValueTask<TOut>> {}