using DragonSpark.Model.Selection;
using DragonSpark.Text;

namespace DragonSpark.Server.Output;

public interface IOutputKeyDefinition<in T> : ISelect<IServiceProvider, IOutputKey<T>>, IText;