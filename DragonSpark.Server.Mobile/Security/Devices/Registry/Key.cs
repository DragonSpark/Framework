using DragonSpark.Application.Compose.Store;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class Key : Key<string>
{
    public static Key Default { get; } = new();

    Key() : base(typeof(Key)) {}
}