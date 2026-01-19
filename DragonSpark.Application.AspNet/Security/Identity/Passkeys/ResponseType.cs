namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed class ResponseType : Text.Text
{
    public static ResponseType Default { get; } = new();

    ResponseType() : base("response") {}
}