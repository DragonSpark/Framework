using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Identity.Twitter.Api;

public interface ITwitterIdentity : IStopAware<string, string?>;