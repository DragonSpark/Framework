using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

public sealed class ValidationIdentity : Storing<ValidationIdentityView?>, IValidationIdentity
{
    public static ValidationIdentity Default { get; } = new();

    ValidationIdentity() : base(ValidationIdentityProcessStore.Default, ValidationIdentityStorageValue.Default) {}
}