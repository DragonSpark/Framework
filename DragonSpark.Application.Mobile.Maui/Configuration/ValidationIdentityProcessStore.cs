using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class ValidationIdentityProcessStore : Variable<ValidationIdentityView>
{
    public static ValidationIdentityProcessStore Default { get; } = new();

    ValidationIdentityProcessStore() {}
}