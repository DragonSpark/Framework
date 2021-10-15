using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication;

public interface IAuthenticate<T> : IOperation<Login<T>> {}