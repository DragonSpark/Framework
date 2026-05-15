namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed record FailedAddPasskeyResult(string Message) : AddPasskeyResult;