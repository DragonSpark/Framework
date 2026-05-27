using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public interface IComposeUserDataInputs<T> : IStopAware<ComposeUserDataInput<T>> where T : class;