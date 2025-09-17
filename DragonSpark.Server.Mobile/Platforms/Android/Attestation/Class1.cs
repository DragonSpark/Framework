using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PlayIntegrity.v1;
using Google.Apis.PlayIntegrity.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

public sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<AndroidPackageSettings>()
                 //
                 .Start<V1Resource>()
                 .Use<ComposeIntegrityService>()
                 .Singleton()
                 //
                 .Then.Start<IProcessIntegrityToken>()
                 .Forward<ProcessIntegrityToken>()
                 .Singleton();
    }
}

public sealed record AndroidPackageSettings
{
    public required string EncodedKey { get; set; }

    public required string PackageName { get; set; }
}

public interface IProcessIntegrityToken : IStopAware<string, IntegrityTokenResult>;

sealed class ProcessIntegrityToken : IProcessIntegrityToken
{
    readonly V1Resource _api;
    readonly string     _package;

    public ProcessIntegrityToken(V1Resource api, AndroidPackageSettings settings) : this(api, settings.PackageName) {}

    public ProcessIntegrityToken(V1Resource api, string package)
    {
        _api     = api;
        _package = package;
    }

    public async ValueTask<IntegrityTokenResult> Get(Stop<string> parameter)
    {
        var (subject, _) = parameter;

        var decode   = _api.DecodeIntegrityToken(new() { IntegrityToken = subject }, _package);
        var response = await decode.ExecuteAsync().Off();
        return new(response.TokenPayloadExternal);
    }
}

sealed class ComposeIntegrityService : IResult<V1Resource>
{
    readonly string[]         _scopes;
    readonly GoogleCredential _root;

    public ComposeIntegrityService(AndroidPackageSettings settings)
        : this(GoogleCredential.FromJson(Base64Decode.Default.Get(settings.EncodedKey)),
               PlayIntegrityService.Scope.Playintegrity) {}

    public ComposeIntegrityService(GoogleCredential root, params string[] scopes)
    {
        _root   = root;
        _scopes = scopes;
    }

    public V1Resource Get()
    {
        var credential  = _root.CreateScoped(_scopes);
        var initializer = new BaseClientService.Initializer { HttpClientInitializer = credential };
        var result      = new PlayIntegrityService(initializer).V1;
        return result;
    }
}

public sealed record IntegrityTokenResult(
    RequestDetails Request,
    ApplicationIntegrity Application,
    DeviceIntegrity Device)
{
    public IntegrityTokenResult(TokenPayloadExternal payload)
        : this(payload.RequestDetails, new(payload.AppIntegrity.AppRecognitionVerdict),
               new(payload.DeviceIntegrity.DeviceRecognitionVerdict)) {}
}

public sealed record DeviceIntegrity(bool IsTrusted, IList<string> Verdict)
{
    public DeviceIntegrity(IList<string> Verdict) : this(Verdict.Contains("MEETS_DEVICE_INTEGRITY"), Verdict) {}
}

public sealed record ApplicationIntegrity(bool IsTrusted, string Verdict)
{
    public ApplicationIntegrity(string Verdict) : this(Verdict == "PLAY_RECOGNIZED", Verdict) {}
}

public readonly record struct VerificationInput(string Challenge, string Input);