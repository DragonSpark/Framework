using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed record AddedPasskeyResult(UserPasskeyInfo Information) : AddPasskeyResult;