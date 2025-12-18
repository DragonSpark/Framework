using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Attestation;

sealed class ClientKeyStorage : Storing<string>
{
    public static ClientKeyStorage Default { get; } = new();

    ClientKeyStorage() 
        : base(ClientKeyStorageValue.Default, GenerateKey.Default.Then().Operation().Out().AsStop()) {}
}