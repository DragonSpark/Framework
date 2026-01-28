using System;

namespace DragonSpark.Server.Mobile.Security.Devices;

public readonly record struct ValidateHashInput(
    DeviceRecord Record,
    ReadOnlyMemory<char> SigningInput,
    ReadOnlyMemory<byte> RawSignature);