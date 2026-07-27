using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Grok.Chat;

public interface IToolRegistration : IResult<Tool>, IStopAware<IReadOnlyDictionary<string, object>, string>;