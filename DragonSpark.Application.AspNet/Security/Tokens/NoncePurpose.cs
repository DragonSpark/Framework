namespace DragonSpark.Application.AspNet.Security.Tokens;

public enum NoncePurpose : byte
{
    Default = 0,
    Jwe     = 1,
    Other   = 2,
}