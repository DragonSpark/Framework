using System;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices;

public readonly record struct ValidatePayloadInput(HttpRequest Request, ReadOnlyMemory<byte> Payload);