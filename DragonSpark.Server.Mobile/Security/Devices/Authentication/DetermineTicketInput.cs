using DragonSpark.Server.Mobile.Security.Devices.Registry;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

public readonly record struct DetermineTicketInput(HttpContext Subject, DeviceRecord Device, string Scheme);