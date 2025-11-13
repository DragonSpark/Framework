using System.Net;
using System.Net.NetworkInformation;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Addresses;

public sealed class DefaultCurrentAddress : FixedSelection<NetworkInterfaceType, IPAddress>
{
    public static DefaultCurrentAddress Default { get; } = new();

    DefaultCurrentAddress() : base(CurrentAddress.Default, NetworkInterfaceType.Ethernet) {}
}