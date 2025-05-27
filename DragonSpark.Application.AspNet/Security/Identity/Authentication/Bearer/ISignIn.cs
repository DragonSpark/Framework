using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public interface ISignIn<T> : IDepending<SignInInput<T>>;