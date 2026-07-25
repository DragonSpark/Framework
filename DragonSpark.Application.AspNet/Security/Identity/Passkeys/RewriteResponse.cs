using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class RewriteResponse : IStopAware<RewriteResponseInput>
{
    readonly DetermineContents       _contents;
    readonly ISelect<string, byte[]> _bytes;

    public RewriteResponse(DetermineContents contents) : this(contents, EncodedTextAsData.Default) {}

    public RewriteResponse(DetermineContents contents, ISelect<string, byte[]> bytes)
    {
        _contents = contents;
        _bytes    = bytes;
    }

    public async ValueTask Get(Stop<RewriteResponseInput> parameter)
    {
        var ((response, stream), stop) = parameter;
        stream.Position                = 0;

        var (content, code) = await _contents.Off(new(new(stream, response.ContentType), stop));
        var bytes = _bytes.Get(content);

        stream.SetLength(0);
        await stream.WriteAsync(bytes, 0, bytes.Length, stop).Off();
        stream.Position = 0;

        response.StatusCode                        = code;
        response.ContentType                       = "application/json; charset=utf-8";
        response.ContentLength                     = bytes.Length;
        response.Headers.CacheControl              = "no-store";
        response.Headers.Pragma                    = "no-cache"; // optional, legacy
        response.Headers.Expires                   = "-1";       // optional
        response.Headers["X-Content-Type-Options"] = "nosniff";  // extra hardening
    }
}