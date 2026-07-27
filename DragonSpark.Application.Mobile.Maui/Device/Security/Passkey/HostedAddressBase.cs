using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using Flurl;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

public class HostedAddressBase : ISelect<string, Uri>
{
    readonly Uri   _root;
    readonly ITime _time;

    protected HostedAddressBase(Uri root) : this(root, Time.Default) {}

    protected HostedAddressBase(Uri root, ITime time)
    {
        _root = root;
        _time = time;
    }

    public Uri Get(string parameter)
    {
        var time = _time.Get().ToUnixTimeMilliseconds();
        return _root.SetQueryParam("r", time).SetFragment($"token={parameter}").ToUri();
    }
}