using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Identity.DeviantArt.Api;

public interface IUserIdentifierQuery : IStopAware<string, string?>;