using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

public sealed class SavedLogin : StorageValue<string>, ISavedLogin
{
    public static SavedLogin Default { get; } = new();

    SavedLogin() : base(A.Type<SavedLogin>().FullName.Verify()) {}
}