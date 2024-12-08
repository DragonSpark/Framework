using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public interface IAuthenticate<T> : IOperation<Login<T>>;