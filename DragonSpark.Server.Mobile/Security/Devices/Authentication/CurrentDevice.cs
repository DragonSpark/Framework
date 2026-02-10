using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Application.AspNet.Security;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class CurrentDevice : IText
{
    readonly ICurrentContext _context;
    readonly IHeader         _header;

    public CurrentDevice(ICurrentContext context, IHeader header)
    {
        _context = context;
        _header  = header;
    }

    public string Get() => _header.Get(_context.Get().Request.Headers).Verify();
}