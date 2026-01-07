using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

public sealed class LoginStatusStorageValue : StorageValue<LoginStatus>, ILoginStatusStorageValue
{
    public static LoginStatusStorageValue Default { get; } = new();

    LoginStatusStorageValue() : base(A.Type<LoginStatusStorageValue>().FullName.Verify()) {}
}