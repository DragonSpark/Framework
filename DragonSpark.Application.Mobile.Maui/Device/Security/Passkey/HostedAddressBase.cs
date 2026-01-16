using System;
using DragonSpark.Model.Selection;
using Flurl;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

public class HostedAddressBase : ISelect<string, Uri>
{
    readonly Uri _root;

    protected HostedAddressBase(Uri root) => _root = root;

    public Uri Get(string parameter) => _root.AppendQueryParam("token", parameter).ToUri();
}