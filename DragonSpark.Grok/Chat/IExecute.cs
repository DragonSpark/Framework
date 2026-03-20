using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Grok.Chat;

public interface IExecute<T> : IStopAware<T, string>;