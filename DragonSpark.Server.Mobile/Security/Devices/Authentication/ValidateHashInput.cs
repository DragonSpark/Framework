using System;
using DragonSpark.Server.Mobile.Security.Devices.Registry;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

public readonly record struct ValidateHashInput(
    DeviceRecord Record,
    ReadOnlyMemory<char> SigningInput,
    ReadOnlyMemory<byte> RawSignature);