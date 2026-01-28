using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices;

public readonly record struct DetermineTicketInput(HttpContext Subject, DeviceRecord Device, string Scheme);