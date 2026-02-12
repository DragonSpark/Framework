using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Application.AspNet.Security;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

public sealed class CurrentDevice : IText
{
    readonly ICurrentContext _context;
    readonly IHeader         _header;
    readonly byte            _length;

    public CurrentDevice(ICurrentContext context)
        : this(context, AuthorizationHeader.Default, SchemeName.Default.Get().Length.Contract().Contract().Next()) {}

    public CurrentDevice(ICurrentContext context, IHeader header, byte length)
    {
        _context = context;
        _header  = header;
        _length  = length;
    }

    public string Get() => _header.Get(_context.Get().Request.Headers).Verify()[_length..];
}