using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public interface IComposeUserDataInputs<T> : IResulting<ComposeUserDataInput<T>> where T : class;