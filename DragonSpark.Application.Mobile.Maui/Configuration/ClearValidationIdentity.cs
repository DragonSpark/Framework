using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

public sealed class ClearValidationIdentity : ClearState<ValidationIdentityView>, IClearValidationIdentity
{
    public static ClearValidationIdentity Default { get; } = new();

    ClearValidationIdentity()
        : base(ValidationIdentityProcessStore.Default, ValidationIdentityStorageValue.Default) {}
}