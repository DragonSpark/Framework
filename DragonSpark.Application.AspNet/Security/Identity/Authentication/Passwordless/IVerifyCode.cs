using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

public interface IVerifyCode<T> : IDepending<VerifyCodeInput<T>> where T : class;