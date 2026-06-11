using DragonSpark.Model.Operations.Selection.Stop.Conditions;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

public interface ISignIn<T> : IDepending<SignInInput<T>>;