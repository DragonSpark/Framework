using DragonSpark.Model.Results;

namespace DragonSpark.Server.Output;

public interface ICurrentTags : IResult<ICollection<string>?>;