using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Communication.Addresses;

public sealed class CurrentAddress : ISelect<NetworkInterfaceType, IPAddress>
{
    public static CurrentAddress Default { get; } = new();

    CurrentAddress() {}

    public IPAddress Get(NetworkInterfaceType parameter)
    {
        var address = NetworkInterface.GetAllNetworkInterfaces()
                                      .Where(x => x.NetworkInterfaceType == parameter &&
                                                  x.OperationalStatus == OperationalStatus.Up)
                                      .SelectMany(x => x.GetIPProperties().UnicastAddresses)
                                      .FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork)
                                      ?.Address;
        var result = address ??
                     throw new
                         InvalidOperationException($"No network adapters with an IPv4 address in the system with type {parameter}!");
        return result;
    }
}