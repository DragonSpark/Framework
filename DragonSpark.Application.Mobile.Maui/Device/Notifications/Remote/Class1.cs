using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Application.Model.Values;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IRegisterDevice>()
                 .Forward<RegisterDevice>()
                 .Singleton()
                 .Then.Start<DeviceRegistration>()
                 .Singleton()
                 .Then.AddSingleton<IMauiInitializeService>(InitializeDeviceRegistration.Default)
                 .TryDecorate<ICompleteLogin, LoginAwareDeviceRegistration>();
    }
}

sealed class LoginAwareDeviceRegistration : ICompleteLogin
{
    readonly ICompleteLogin  _previous;
    readonly IRegisterDevice _register;

    public LoginAwareDeviceRegistration(ICompleteLogin previous, IRegisterDevice register)
    {
        _previous = previous;
        _register = register;
    }

    public async ValueTask Get(Stop<AccessTokenView?> parameter)
    {
        await _previous.Off(parameter);
        await _register.Off(parameter);
    }
}

sealed class DeviceRegistration : IStopAware
{
    readonly IRegisterDevice             _register;
    readonly ILogger<DeviceRegistration> _log;

    public DeviceRegistration(IRegisterDevice register, ILogger<DeviceRegistration> log)
    {
        _register = register;
        _log      = log;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromDays(1));
        var       run  = new Switch(true);
        while (!parameter.IsCancellationRequested && run)
        {
            try
            {
                while (await timer.WaitForNextTickAsync(parameter).Off())
                {
                    await _register.Off(parameter);
                }
            }
            catch (OperationCanceledException) {}
            catch (Exception ex)
            {
                _log.LogCritical(ex, "A problem was encountered when refreshing device registration");
                run.Down();
            }
        }
    }
}

sealed class InitializeDeviceRegistration : IMauiInitializeService
{
    public static InitializeDeviceRegistration Default { get; } = new();

    InitializeDeviceRegistration() {}

    public void Initialize(IServiceProvider services)
    {
        _ = services.GetRequiredService<DeviceRegistration>().Get(CancellationToken.None);
    }
}

public interface IDeviceIdentifier : IText;

public sealed record ActionReceivedMessage(string Action);

public sealed record NewTokenReceivedMessage(string Token);

public sealed class SaveDeviceToken : SaveState<string>
{
    public static SaveDeviceToken Default { get; } = new();

    SaveDeviceToken() : base(DeviceTokenProcessStore.Default, DeviceTokenStorage.Default) {}
}

public sealed class DeviceToken : Storing<string?>
{
    public static DeviceToken Default { get; } = new();

    DeviceToken() : base(DeviceTokenProcessStore.Default, DeviceTokenStorage.Default) {}
}

sealed class DeviceTokenStorage : StorageValue<string>
{
    public static DeviceTokenStorage Default { get; } = new();

    DeviceTokenStorage() {}
}

public sealed class DeviceTokenProcessStore : Variable<string>
{
    public static DeviceTokenProcessStore Default { get; } = new();

    DeviceTokenProcessStore() {}
}

public sealed class ActionKey : Text.Text
{
    public static ActionKey Default { get; } = new();

    ActionKey() : base("action") {}
}

public interface IRegisterDeviceToken : DragonSpark.Model.Operations.Stop.IStopAware<string>;

public interface IRegisterDevice : IStopAware;

sealed class RegisterDevice : IRegisterDevice
{
    readonly IRegisterDeviceToken                                          _register;
    readonly DragonSpark.Model.Operations.Results.Stop.IStopAware<string?> _token;

    public RegisterDevice(IRegisterDeviceToken register) : this(register, DeviceToken.Default) {}

    public RegisterDevice(IRegisterDeviceToken register,
                          DragonSpark.Model.Operations.Results.Stop.IStopAware<string?> token)
    {
        _register = register;
        _token    = token;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        var token = await _token.Off(parameter);
        if (token is not null)
        {
            await _register.Off(new(token, parameter));
        }
    }
}

/*public class PushRegistrationService : IDisposable
{
    private readonly HttpClient _httpClient; // or your API service
    private readonly string     _installationId;
    private readonly string     _userId;

    private PeriodicTimer?           _slideTimer;
    private CancellationTokenSource? _cts;

    public PushRegistrationService(HttpClient httpClient, string installationId, string userId)
    {
        _httpClient     = httpClient;
        _installationId = installationId;
        _userId         = userId;
    }

    // Call this after successful first registration (e.g. after getting push token)
    public void StartSlidingTimer()
    {
        StopSlidingTimer(); // safety

        _cts        = new CancellationTokenSource();
        _slideTimer = new PeriodicTimer(TimeSpan.FromHours(24));

        _ = Task.Run(async () =>
                     {
                         try
                         {
                             while (await _slideTimer.WaitForNextTickAsync(_cts.Token))
                             {
                                 await SlideRegistrationAsync();
                             }
                         }
                         catch (OperationCanceledException) {}
                         catch (Exception ex)
                         {
                             // Log error - don't crash the app
                             System.Diagnostics.Debug.WriteLine($"Slide timer error: {ex}");
                         }
                     });
    }

    private async Task SlideRegistrationAsync()
    {
        try
        {
            // Re-fetch current push token if needed (recommended)
            var currentToken = await GetCurrentPushTokenAsync();

            await _httpClient.PostAsJsonAsync("api/push/register", new
            {
                InstallationId = _installationId,
                PushToken      = currentToken,
                UserId         = _userId,
                Platform       = DeviceInfo.Platform == DevicePlatform.iOS ? "ios" : "android"
            });

            System.Diagnostics.Debug.WriteLine("Successfully slid registration (extended TTL)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to slide registration: {ex}");
        }
    }

    // Implement this based on your push setup (returns APNs token or FCM token)
    private async Task<string> GetCurrentPushTokenAsync()
    {
        // Call your push plugin/service to get latest token
        return "current-token-here";
    }

    public void StopSlidingTimer()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _slideTimer?.Dispose();
        _cts        = null;
        _slideTimer = null;
    }

    public void Dispose() => StopSlidingTimer();
}*/