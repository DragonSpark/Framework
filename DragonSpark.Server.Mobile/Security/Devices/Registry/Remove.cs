using DragonSpark.Application.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class Remove : RemoveFromMemory<string>
{
    public Remove(IMemoryCache memory) : base(memory, Key.Default) {}
}