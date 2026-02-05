using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Mobile.Maui.Storage;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class ValidationIdentityStorageValue : StorageValue<ValidationIdentityView>
{
    public static ValidationIdentityStorageValue Default { get; } = new();

    ValidationIdentityStorageValue() {}
}