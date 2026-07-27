namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

public interface IValidationRecord
{
    Guid Identity { get; set; }

    string KeyHash { get; set; }

    DateTimeOffset Created { get; set; }

    string Thumbprint { get; set; }
}