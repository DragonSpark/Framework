using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public interface IAuthenticate<T> : IStopAware<Login<T>>;