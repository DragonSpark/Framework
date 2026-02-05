using DragonSpark.Application.AspNet.Entities.Queries.Composition;

namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

sealed class SelectValidatedDevice : StartWhere<string, ValidationRecordBase>
{
    public static SelectValidatedDevice Default { get; } = new();

    SelectValidatedDevice() : base((p, x) => x.Thumbprint == p) {}
}