using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Biometric;
using Exception = System.Exception;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Biometrics;

sealed class RequestUserBiometric : IRequestUserBiometric
{
    readonly IBiometric            _service;
    readonly BiometricsError       _log;
    readonly AuthenticationRequest _request;

    public RequestUserBiometric(BiometricsError log, AuthenticationRequest request)
        : this(BiometricAuthenticationService.Default, log, request) {}

    public RequestUserBiometric(IBiometric service, BiometricsError log, AuthenticationRequest request)
    {
        _service = service;
        _log     = log;
        _request = request;
    }

    public async ValueTask<bool> Get(Stop<None> parameter)
    {
        var (_, stop) = parameter;
        var status = await _service.GetAuthenticationStatusAsync().On();
        switch (status)
        {
            case BiometricHwStatus.PresentButNotEnrolled:
            case BiometricHwStatus.NotEnrolled:
            case BiometricHwStatus.NoHardware:
            case BiometricHwStatus.Unsupported:
            case BiometricHwStatus.Unavailable:
                return true;
            case BiometricHwStatus.Success:
                try
                {
                    var result = await _service.AuthenticateAsync(_request, stop).Off();
                    return result.Status == BiometricResponseStatus.Success;
                }
                catch (TaskCanceledException)
                {
                    return false;
                }
                catch (Exception e)
                {
                    _log.Execute(e);
                    return false;
                }
            case BiometricHwStatus.LockedOut:
            case BiometricHwStatus.Failure:
            default:
                return false;
        }
    }

    public sealed class BiometricsError : LogErrorException
    {
        public BiometricsError(ILogger<BiometricsError> logger)
            : base(logger, "There was a problem while handling the biometrics for user") {}
    }
}