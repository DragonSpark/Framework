using System;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.Security.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Runtime;
using DragonSpark.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetFabric.Hyperlinq;

namespace DragonSpark.Server.Mobile.Security;

class Class1;

public interface IDeviceRegistry : IStopAware<string, DeviceRecord?>;

sealed class DeviceRegistry : EvaluateToSingleOrDefault<string, DeviceRecord>, IDeviceRegistry
{
    public DeviceRegistry(IScopes scopes) : base(scopes, SelectDeviceRecord.Default) {}
}

sealed class SelectDeviceRecord : StartWhereSelect<string, DeviceKey, DeviceRecord>
{
    public static SelectDeviceRecord Default { get; } = new();

    SelectDeviceRecord()
        : base((p, x) => x.DeviceId == p,
               x => new(x.DeviceId, x.Kty, x.Crv, x.X, x.Y, x.IsBlocked, x.AttestedAtUtc, x.LastSeenAtUtc,
                        x.EvaluationType)) {}
}

public readonly record struct BlockInput(string DeviceId, bool Blocked);

public readonly record struct DeviceUsedInput(string DeviceId, DateTime Now);

public readonly record struct UpdateKeysInput(DeviceRecord Subject, UpdateSettersBuilder<DeviceKey> Builder);

sealed class UpdateKeys : ICommand<UpdateKeysInput>
{
    public static UpdateKeys Default { get; } = new();

    UpdateKeys() : this(Time.Default) {}

    readonly ITime _time;

    public UpdateKeys(ITime time)
    {
        _time = time;
    }

    public void Execute(UpdateKeysInput parameter)
    {
        var (key, builder) = parameter;
        var now = _time.Get().UtcDateTime;
        builder.SetProperty(d => d.Kty, _ => key.Kty)
               .SetProperty(d => d.Crv, _ => key.Crv)
               .SetProperty(d => d.X, _ => key.X)
               .SetProperty(d => d.Y, _ => key.Y)
               .SetProperty(d => d.EvaluationType, _ => key.EvaluationType)
               .SetProperty(d => d.AttestedAtUtc, _ => key.AttestedAtUtc ?? now)
               .SetProperty(d => d.LastSeenAtUtc, _ => key.LastSeenAtUtc ?? now);
    }
}

public interface IUpsertDevice : IDepending<DeviceRecord>;

sealed class ExceptionAwareAddRecord : IDepending<DeviceRecord>
{
    readonly AddRecord                        _previous;
    readonly ILogger<ExceptionAwareAddRecord> _logger;
    readonly UpdateDevice                     _update;

    public ExceptionAwareAddRecord(AddRecord previous, ILogger<ExceptionAwareAddRecord> logger, UpdateDevice update)
    {
        _previous = previous;
        _logger   = logger;
        _update   = update;
    }

    public async ValueTask<bool> Get(Stop<DeviceRecord> parameter)
    {
        try
        {
            return await _previous.Off(parameter);
        }
        catch (DbUpdateException ex) when (IsDuplicate.Default.Get(ex))
        {
            var (record, _) = parameter;
            _logger.LogDebug("Upsert race for {DeviceId}; retrying UPDATE.", record.DeviceId);
            return await _update.Off(parameter);
        }
    }
}

sealed class AddRecord : IDepending<DeviceRecord>
{
    readonly Editors _editors;
    readonly ITime   _time;

    public AddRecord(Editors editors) : this(editors, Time.Default) {}

    public AddRecord(Editors editors, ITime time)
    {
        _editors = editors;
        _time    = time;
    }

    public async ValueTask<bool> Get(Stop<DeviceRecord> parameter)
    {
        var (r, stop) = parameter;
        using var editor = _editors.Get(stop);
        var       now    = _time.Get().UtcDateTime;
        editor.Add(new DeviceKey
        {
            DeviceId       = r.DeviceId, Kty                       = r.Kty, Crv = r.Crv, X = r.X, Y = r.Y,
            IsBlocked      = r.IsBlocked, CreatedAtUtc             = now,
            AttestedAtUtc  = r.AttestedAtUtc ?? now, LastSeenAtUtc = r.LastSeenAtUtc ?? now,
            EvaluationType = r.EvaluationType
        });
        await editor.Off();
        return true;
    }
}

sealed class UpdateDevice : IDepending<DeviceRecord>
{
    readonly INewContext               _context;
    readonly ICommand<UpdateKeysInput> _update;

    public UpdateDevice(INewContext context) : this(context, UpdateKeys.Default) {}

    public UpdateDevice(INewContext context, ICommand<UpdateKeysInput> update)
    {
        _context = context;
        _update  = update;
    }

    public async ValueTask<bool> Get(Stop<DeviceRecord> parameter)
    {
        var (r, stop) = parameter;
        await using var db   = _context.Get();
        var             keys = db.Set<DeviceKey>();
        var updated = await keys.Where(d => d.DeviceId == r.DeviceId)
                                .ExecuteUpdateAsync(x => _update.Execute(new(r, x)), stop)
                                .Off();
        return updated > 0;
    }
}

sealed class UpsertDevice : IUpsertDevice
{
    readonly UpdateDevice            _update;
    readonly ExceptionAwareAddRecord _add;

    public UpsertDevice(UpdateDevice update, ExceptionAwareAddRecord add)
    {
        _update = update;
        _add    = add;
    }

    public async ValueTask<bool> Get(Stop<DeviceRecord> parameter)
    {
        return await _update.Off(parameter) || await _add.Off(parameter);
    }
}

public interface IBlockDevice : IDepending<BlockInput>;

public interface IDeviceUsed : IDepending<DeviceUsedInput>;

public sealed record DeviceRecord(
    string DeviceId,
    string Kty,
    string Crv,
    string X,
    string Y,
    bool IsBlocked,
    DateTime? AttestedAtUtc,
    DateTime? LastSeenAtUtc,
    string? EvaluationType);

public enum DPoPNonceType : byte
{
    DPoP          = 0,
    PasskeyTicket = 1,
    PasskeyJwe    = 2
}

[Index(nameof(IsBlocked)), Index(nameof(LastSeenAtUtc))]
public sealed class DeviceKey
{
    [Key, MaxLength(64)]
    public string DeviceId { get; set; } = default!; // RFC7638 JWK thumbprint (base64url)

    [MaxLength(8)]
    public string Kty { get; set; } = "EC";

    [MaxLength(16)]
    public string Crv { get; set; } = "P-256";

    [MaxLength(128)]
    public string X { get; set; } = default!;

    [MaxLength(128)]
    public string Y { get; set; } = default!;

    public bool IsBlocked { get; set; }

    public required DateTime CreatedAtUtc { get; init; }

    public DateTime? AttestedAtUtc { get; set; }
    public DateTime? LastSeenAtUtc { get; set; }

    [MaxLength(32)]
    public string? EvaluationType { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}

[Index(nameof(ExpiresAtUtc)), Index(nameof(Type), nameof(ExpiresAtUtc))]
public sealed class DPoPNonce
{
    [Key, MaxLength(64)]
    public string Nonce { get; set; } = default!; // base64url 192-bit (PRIMARY KEY)

    public DateTime IssuedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }

    [MaxLength(256)]
    public string? Scope { get; set; } // optional: https://host/path or flow label

    public DPoPNonceType Type { get; set; }
    public DateTime? UsedAtUtc { get; set; }
}

/*public class SecurityDbContext : DbContext
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options) {}

    public DbSet<DeviceKey> DeviceKeys { get; }

    public DbSet<DPoPNonce> DPoPNonces { get; }
}*/

public readonly record struct IssueNonceInput(HttpContext Context, DPoPNonceType Type);

sealed class AddNonce : IStopAware<IssueNonceInput, string>
{
    readonly Editors _editors;
    readonly IText   _nonce;
    readonly ITime   _time;

    public AddNonce(Editors editors) : this(editors, DefaultFormattedNonces.Default, Time.Default) {}

    public AddNonce(Editors editors, IText nonce, ITime time)
    {
        _editors = editors;
        _nonce   = nonce;
        _time    = time;
    }

    public async ValueTask<string> Get(Stop<IssueNonceInput> parameter)
    {
        var ((context, type), stop) = parameter;
        using var editor = _editors.Get(stop);
        var       result = _nonce.Get();
        var       now    = _time.Get().UtcDateTime;
        var       scope  = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";

        editor.Add(new DPoPNonce
        {
            Nonce        = result, Type = type, IssuedAtUtc = now, Scope = scope,
            ExpiresAtUtc = now + DefaultExpiration.Default
        });

        await editor.Off();
        return result;
    }
}

public interface IIssueNonce : ISelecting<IssueNonceInput, string>;

sealed class IssueNonce : IIssueNonce
{
    readonly AddNonce            _add;
    readonly ILogger<IssueNonce> _logger;

    public IssueNonce(AddNonce add, ILogger<IssueNonce> logger)
    {
        _add    = add;
        _logger = logger;
    }

    public async ValueTask<string> Get(IssueNonceInput parameter)
    {
        try
        {
            return await _add.Off(new(parameter, parameter.Context.RequestAborted));
        }
        catch (DbUpdateException ex) when (IsDuplicate.Default.Get(ex))
        {
            _logger.LogWarning("Nonce collision; regenerating");
            return await _add.Off(new(parameter, parameter.Context.RequestAborted));
        }
    }
}

public readonly record struct MarkUsedInput(string Identity, DPoPNonceType Type);

public interface IMarkUsed : IDepending<MarkUsedInput>;

sealed class MarkUsed : IMarkUsed
{
    readonly INewContext           _context;
    readonly TypeAwareComposeQuery _query;

    public MarkUsed(INewContext context, TypeAwareComposeQuery query)
    {
        _context = context;
        _query   = query;
    }

    public async ValueTask<bool> Get(Stop<MarkUsedInput> parameter)
    {
        var ((identity, type), stop) = parameter;
        if (!identity.IsNullOrWhiteSpace())
        {
            await using var context = _context.Get();
            var (query, now) = _query.Get(new(context.Set<DPoPNonce>(), identity, type));
            var rows = await query.ExecuteUpdateAsync(s => s.SetProperty(n => n.UsedAtUtc, _ => now), stop).Off();
            return rows == 1;
        }

        return false;
    }
}

public readonly record struct ComposeQueryInput(DbSet<DPoPNonce> Source, string Identity, DPoPNonceType? Type);

public readonly record struct ComposeQueryResult(IQueryable<DPoPNonce> Query, DateTime Now);

public interface IComposeQuery : ISelect<ComposeQueryInput, ComposeQueryResult> {}

sealed class ComposeQuery : IComposeQuery
{
    public static ComposeQuery Default { get; } = new();

    ComposeQuery() : this(Time.Default) {}

    readonly ITime _time;

    public ComposeQuery(ITime time)
    {
        _time = time;
    }

    public ComposeQueryResult Get(ComposeQueryInput parameter)
    {
        var (source, identity, _) = parameter;
        var now = _time.Get().UtcDateTime;
        return new(source.Where(n => n.Nonce == identity && n.UsedAtUtc == null && n.ExpiresAtUtc >= now), now);
    }
}

sealed class TypeAwareComposeQuery : IComposeQuery
{
    readonly ComposeQuery _previous;

    public TypeAwareComposeQuery(ComposeQuery previous)
    {
        _previous = previous;
    }

    public ComposeQueryResult Get(ComposeQueryInput parameter)
    {
        var (_, _, type) = parameter;
        var previous = _previous.Get(parameter);
        return type is not null ? previous with { Query = previous.Query.Where(n => n.Type == type.Value) } : previous;
    }
}

sealed class IsDuplicate : Condition<Exception>
{
    public static IsDuplicate Default { get; } = new();

    IsDuplicate() : base(x => x.InnerException is SqlException { Number: 2627 or 2601 }) {}
}

sealed class DPoPNonceCleanupService : BackgroundService
{
    readonly CleanUpNonces                    _clean;
    readonly TimeSpan                         _interval;
    readonly ILogger<DPoPNonceCleanupService> _logger;

    public DPoPNonceCleanupService(CleanUpNonces clean, ILogger<DPoPNonceCleanupService> logger)
        : this(clean, TimeSpan.FromMinutes(10), logger) {}

    public DPoPNonceCleanupService(CleanUpNonces clean, TimeSpan interval, ILogger<DPoPNonceCleanupService> logger)
    {
        _clean    = clean;
        _interval = interval;
        _logger   = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_interval);
        while (await timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false))
        {
            try
            {
                var deleted = await _clean.Off(stoppingToken);
                if (deleted > 0)
                {
                    _logger.LogDebug("Nonce cleanup removed {Count} rows", deleted);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Nonce cleanup failed");
            }
        }
    }
}

sealed class CleanUpNonces : IStopAware<uint>
{
    readonly INewContext _context;
    readonly ITime       _time;

    public CleanUpNonces(INewContext context) : this(context, Time.Default) {}

    public CleanUpNonces(INewContext context, ITime time)
    {
        _context = context;
        _time    = time;
    }

    public async ValueTask<uint> Get(CancellationToken parameter)
    {
        await using var db  = _context.Get();
        var             now = _time.Get().UtcDateTime;
        return (uint)await db.Set<DPoPNonce>()
                             .Where(n => n.UsedAtUtc != null && n.ExpiresAtUtc < now)
                             .ExecuteDeleteAsync(parameter)
                             .Off();
    }
}

public sealed class DevicePoPOptions : AuthenticationSchemeOptions
{
    public TimeSpan MaxSkew { get; set; } = TimeSpan.FromSeconds(60);
    public bool RequireNonce { get; set; } = true; // banking-grade
}

/*
sealed class DetermineRecordIdentifier : IStopAware<HttpContext, string?>
{
    public ValueTask<string?> Get(Stop<HttpContext> parameter)
    {
        /*var ((subject, scheme), stop) = parameter;
        var auth = subject.Request.Headers.Authorization.ToString();
        if (!auth.IsNullOrWhiteSpace() && auth.StartsWith("DevicePoP ", StringComparison.OrdinalIgnoreCase))
        {
            var deviceId = auth["DevicePoP ".Length..].Trim();
            if (!deviceId.IsNullOrWhiteSpace())
            {
                var record = await _devices.Off(new(deviceId, stop));
                if (record is { IsBlocked: false })
        return default;#1#
    }
}
*/

public interface IValidation<TIn, TOut> : IStopAware<TIn, ValidationResult<TOut>>;

public sealed record ValidationResult<T>(T? Instance, AuthenticateResult? Result)
{
    public static implicit operator ValidationResult<T>(AuthenticateResult instance)
    {
        return new ValidationResult<T>(default, instance);
    }

    public static implicit operator ValidationResult<T>(T instance)
    {
        return new ValidationResult<T>(instance, null);
    }
}

sealed class DetermineRecord : IStopAware<HttpContext, DeviceRecord?>
{
    public ValueTask<DeviceRecord?> Get(Stop<HttpContext> parameter)
    {
        return default;
    }
}

public readonly record struct HandleAuthenticationInput(HttpContext Context, string Scheme);

public readonly record struct DetermineTicketInput(HttpContext Subject, DeviceRecord Device, string Scheme);

sealed class ValidateHeader : IParser<AuthenticateResult?>
{
    public static ValidateHeader Default { get; } = new();

    ValidateHeader() : this(StringComparison.OrdinalIgnoreCase) {}

    readonly StringComparison _comparison;

    public ValidateHeader(StringComparison comparison)
    {
        _comparison = comparison;
    }

    public AuthenticateResult? Get(string parameter)
    {
        using var document = JsonDocument.Parse(parameter);
        var       header   = document.RootElement;
        return header.TryGetProperty("typ", out var typ) && string.Equals(typ.GetString(), "dpop+jwt", _comparison)
                   ? !header.TryGetProperty("alg", out var alg) || !string.Equals(alg.GetString(), "ES256", _comparison)
                         ? AuthenticateResult.Fail("Invalid alg")
                         : null
                   : AuthenticateResult.Fail("Invalid typ");
    }
}

public readonly record struct ValidateHashInput(DeviceRecord Record, string SigningInput, byte[] RawSignature);

sealed class ValidateHash : ISelect<ValidateHashInput, AuthenticateResult?>
{
    public static ValidateHash Default { get; } = new();

    ValidateHash() : this(AuthenticateResult.Fail("Invalid DPoP signature")) {}

    readonly AuthenticateResult _result;

    public ValidateHash(AuthenticateResult result) => _result = result;

    public AuthenticateResult? Get(ValidateHashInput parameter)
    {
        var (record, signingInput, bytes) = parameter;
        using var ecdsa  = CreateEcdsa.Default.Get(new(record.X, record.Y));
        var       digest = SHA256.HashData(Encoding.ASCII.GetBytes(signingInput));
        using var derSig = JoseToDer.Default.Get(bytes);
        return ecdsa.VerifyHash(digest, derSig.Memory.Span) ? null : _result;
    }
}

public readonly record struct ValidatePayloadInput(HttpRequest Request, string Payload);

sealed class ValidatePayload : IStopAware<ValidatePayloadInput, AuthenticateResult?>
{
    readonly IOptionsMonitor<DevicePoPOptions> _options;
    readonly IMarkUsed                         _mark;

    public ValidatePayload(IOptionsMonitor<DevicePoPOptions> options, IMarkUsed mark)
    {
        _options = options;
        _mark    = mark;
    }

    public async ValueTask<AuthenticateResult?> Get(Stop<ValidatePayloadInput> parameter)
    {
        var ((request, payload), stop) = parameter;
        using var document = JsonDocument.Parse(payload);
        var       root     = document.RootElement;
        if (root.TryGetProperty("htm", out var htm) && root.TryGetProperty("htu", out var htu) &&
            root.TryGetProperty("iat", out var iatEl))
        {
            if (long.TryParse(iatEl.ToString(), out var iat))
            {
                var method = htm.GetString();
                if (string.Equals(method, request.Method, StringComparison.OrdinalIgnoreCase))
                {
                    var url         = htu.GetString();
                    var expectedHtu = $"{request.Scheme}://{request.Host}{request.Path}";
                    if (string.Equals(url, expectedHtu, StringComparison.Ordinal))
                    {
                        var now = _options.CurrentValue.TimeProvider.Verify().GetUtcNow().ToUnixTimeSeconds();
                        if (Math.Abs(now - iat) > _options.CurrentValue.MaxSkew.TotalSeconds)
                        {
                            return AuthenticateResult.Fail("DPoP iat too old");
                        }

                        if (_options.CurrentValue.RequireNonce)
                        {
                            if (!root.TryGetProperty("nonce", out var nonceEl))
                            {
                                return AuthenticateResult.Fail("Nonce required");
                            }

                            if (!await _mark.Off(new(new(nonceEl.GetString().EmptyIfNull(), DPoPNonceType.DPoP), stop)))
                            {
                                return AuthenticateResult.Fail("Nonce invalid/reused");
                            }
                        }

                        return null;
                    }

                    return AuthenticateResult.Fail("htu mismatch");
                }

                return AuthenticateResult.Fail("htm mismatch");
            }

            return AuthenticateResult.Fail("Invalid iat");
        }

        return AuthenticateResult.Fail("Invalid DPoP payload");
    }
}

sealed class DetermineTicket : IStopAware<DetermineTicketInput, AuthenticateResult>
{
    readonly OptionsAwareApplyNonce _apply;
    readonly ValidatePayload        _payload;

    public DetermineTicket(OptionsAwareApplyNonce apply, ValidatePayload payload)
    {
        _apply   = apply;
        _payload = payload;
    }

    public async ValueTask<AuthenticateResult> Get(Stop<DetermineTicketInput> parameter)
    {
        var ((subject, record, scheme), stop) = parameter;
        await _apply.Off(new(new(subject, DPoPNonceType.DPoP), stop));

        var dpop   = subject.Request.Headers["DPoP"].ToString();
        var parsed = JwsParser.Default.Get(dpop);
        if (parsed is not null)
        {
            var (hdrJson, plJson, signingInput, sigRaw) = parsed.Value;

            return ValidateHeader.Default.Get(hdrJson)
                   ?? ValidateHash.Default.Get(new(record, signingInput, sigRaw))
                   ?? await _payload.Off(new(new(subject.Request, plJson), stop))
                   ?? SuccessfulTicket.Default.Get(new(record.DeviceId, scheme));
        }

        return AuthenticateResult.Fail("Invalid or missing DPoP JWS");
    }
}

sealed class HandleAuthentication : IStopAware<HandleAuthenticationInput, AuthenticateResult>
{
    readonly IDeviceRegistry _devices;
    readonly DetermineTicket _ticket;

    public HandleAuthentication(IDeviceRegistry devices, DetermineTicket ticket)
    {
        _devices = devices;
        _ticket  = ticket;
    }

    public async ValueTask<AuthenticateResult> Get(Stop<HandleAuthenticationInput> parameter)
    {
        var ((subject, scheme), stop) = parameter;
        var auth = subject.Request.Headers.Authorization.ToString();
        if (!auth.IsNullOrWhiteSpace() && auth.StartsWith("DevicePoP ", StringComparison.OrdinalIgnoreCase))
        {
            var deviceId = auth["DevicePoP ".Length..].Trim();
            if (!deviceId.IsNullOrWhiteSpace())
            {
                var record = await _devices.Off(new(deviceId, stop));
                return record is { IsBlocked: false }
                           ? await _ticket.Off(new(new(subject, record, scheme), stop))
                           : AuthenticateResult.Fail("Unknown/blocked device");
            }

            return AuthenticateResult.Fail("Missing device id");
        }

        return AuthenticateResult.NoResult();
    }
}

sealed class DevicePoPHandler : AuthenticationHandler<DevicePoPOptions>
{
    readonly HandleAuthentication _handle;

    // ReSharper disable once TooManyDependencies
    public DevicePoPHandler(IOptionsMonitor<DevicePoPOptions> options, ILoggerFactory logger, UrlEncoder encoder,
                            HandleAuthentication handle)
        : base(options, logger, encoder)
    {
        _handle = handle;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return _handle.Allocate(new(new(Context, Scheme.Name), Context.RequestAborted));
    }
}

public readonly record struct SuccessfulTicketInput(string Device, string Scheme);

sealed class SuccessfulTicket : ISelect<SuccessfulTicketInput, AuthenticateResult>
{
    public static SuccessfulTicket Default { get; } = new();

    SuccessfulTicket() : this(new(ClaimTypes.AuthenticationMethod, "DevicePoP")) {}

    readonly Claim _method;

    public SuccessfulTicket(Claim method) => _method = method;

    public AuthenticateResult Get(SuccessfulTicketInput parameter)
    {
        var (device, scheme) = parameter;
        var identity = new ClaimsIdentity(new[] { new("device_id", device), _method }, scheme);
        return AuthenticateResult.Success(new AuthenticationTicket(new(identity), scheme));
    }
}

sealed class OptionsAwareApplyNonce : Model.Operations.Stop.IStopAware<IssueNonceInput>
{
    readonly IOptions<DevicePoPOptions> _options;
    readonly ApplyNonce                 _previous;

    public OptionsAwareApplyNonce(IOptions<DevicePoPOptions> options, ApplyNonce previous)
    {
        _options  = options;
        _previous = previous;
    }

    public ValueTask Get(Stop<IssueNonceInput> parameter)
        => _options.Value.RequireNonce ? _previous.Get(parameter) : ValueTask.CompletedTask;
}

sealed class ApplyNonce : Model.Operations.Stop.IStopAware<IssueNonceInput>
{
    readonly IIssueNonce _previous;
    readonly string      _header;

    public ApplyNonce(IIssueNonce previous) : this(previous, DpopNonceHeaderName.Default) {}

    public ApplyNonce(IIssueNonce previous, string header)
    {
        _previous = previous;
        _header   = header;
    }

    public async ValueTask Get(Stop<IssueNonceInput> parameter)
    {
        var (subject, _)                          = parameter;
        subject.Context.Response.Headers[_header] = await _previous.Off(parameter);
    }
}

sealed class DpopNonceHeaderName : Text.Text
{
    public static DpopNonceHeaderName Default { get; } = new();

    DpopNonceHeaderName() : base("DPoP-Nonce") {}
}

sealed class Base64UrlDecode : IFormatter<ReadOnlyMemory<char>>
{
    public static Base64UrlDecode Default { get; } = new();

    Base64UrlDecode() : this(NewLeasing<byte>.Default, new UTF8Encoding(false, true)) {}

    readonly INewLeasing<byte> _new;
    readonly Encoding          _encoding;

    public Base64UrlDecode(INewLeasing<byte> @new, Encoding encoding)
    {
        _new      = @new;
        _encoding = encoding;
    }

    public string Get(ReadOnlyMemory<char> parameter)
    {
        if (parameter.Length != 0)
        {
            using var lease       = _new.Get((parameter.Length + 3) / 4 * 3);
            var       span        = lease.AsSpan();
            var       destination = span[..lease.Length.Degrade()];
            if (Base64Url.TryDecodeFromChars(parameter.Span, destination, out var written))
            {
                CryptographicOperations.ZeroMemory(span[..written]);
                return _encoding.GetString(destination[..written]);
            }

            throw new FormatException("Invalid base64url input.");
        }

        return string.Empty;
    }
}

sealed class JwsParser : IParser<JwsResult?>
{
    public static JwsParser Default { get; } = new();

    JwsParser() : this(Base64UrlDecode.Default, ComposeJwsParserInput.Default) {}

    readonly IFormatter<ReadOnlyMemory<char>> _decode;
    readonly IParser<JwsParserInput?>         _input;

    public JwsParser(IFormatter<ReadOnlyMemory<char>> decode, IParser<JwsParserInput?> input)
    {
        _decode = decode;
        _input  = input;
    }

    public JwsResult? Get(string parameter)
    {
        var input = _input.Get(parameter);
        if (input is not null)
        {
            var (first, next, second, all) = input.Value;
            var signingInput = parameter[..all]; // "<hdr>.<pl>"
            try
            {
                var memory   = parameter.AsMemory();
                var rest     = memory[next..];
                var hdrJson  = _decode.Get(memory[..first]);
                var plJson   = _decode.Get(rest[..second]);
                var sigBytes = Base64Url.DecodeFromChars(rest[(second + 1)..].Span);
                return new(hdrJson, plJson, signingInput, sigBytes);
            }
            catch (Exception e) when (e is DecoderFallbackException or FormatException or ArgumentException)
            {
                return null;
            }
        }

        return null;
    }
}

public readonly record struct JwsParserInput(int First, int Next, int Second, int All)
{
    public JwsParserInput(int First, int Second) : this(First, First + 1, Second) {}

    public JwsParserInput(int First, int Next, int Second) : this(First, Next, Second, Next + Second) {}
}

sealed class ComposeJwsParserInput : IParser<JwsParserInput?>
{
    public static ComposeJwsParserInput Default { get; } = new();

    ComposeJwsParserInput() {}

    public JwsParserInput? Get(string parameter)
    {
        if (!parameter.IsNullOrWhiteSpace())
        {
            var span  = parameter.AsSpan();
            var first = span.IndexOf('.');
            if (first > 0)
            {
                var rest   = span.Slice(first + 1);
                var second = rest.IndexOf('.');
                if (second > 0)
                {
                    return new(first, second);
                }
            }
        }

        return null;
    }
}

public readonly record struct JwsResult(string HdrJson, string PlJson, string SigningInput, byte[] RawSignature);

public readonly record struct CreateEcdsaInput(string X, string Y);

sealed class CreateEcdsa : ISelect<CreateEcdsaInput, ECDsa>
{
    public static CreateEcdsa Default { get; } = new();

    CreateEcdsa() {}

    public ECDsa Get(CreateEcdsaInput parameter)
    {
        var (xUrl, yUrl) = parameter;
        var parameters = new ECParameters
        {
            Curve = ECCurve.NamedCurves.nistP256,
            Q     = new ECPoint { X = WebEncoders.Base64UrlDecode(xUrl), Y = WebEncoders.Base64UrlDecode(yUrl) }
        };
        return ECDsa.Create(parameters);
    }
}

sealed class JoseToDer : ISelect<byte[], Lease<byte>>
{
    public static JoseToDer Default { get; } = new();

    JoseToDer() : this(IntegerToDer.Default, NewLeasing<byte>.Default) {}

    readonly ISelect<ReadOnlyMemory<byte>, Lease<byte>> _der;
    readonly INewLeasing<byte>                          _new;

    public JoseToDer(ISelect<ReadOnlyMemory<byte>, Lease<byte>> der, INewLeasing<byte> @new)
    {
        _der = der;
        _new = @new;
    }

    public Lease<byte> Get(byte[] parameter)
    {
        switch (parameter.Length)
        {
            case 64:
            {
                using var derR   = _der.Get(parameter.AsMemory(0, 32));
                using var derS   = _der.Get(parameter.AsMemory(32, 32));
                var       result = _new.Get(2 + derR.Length + derS.Length);
                result.Store[0] = 0x30;
                result.Store[1] = (byte)(derR.Length + derS.Length);
                Buffer.BlockCopy(derR.Rented, 0, result.Store, 2, derR.Length);
                Buffer.BlockCopy(derS.Rented, 0, result.Store, 2 + derR.Length, derS.Length);
                return result.AsEnumerable();
            }
            default:
                throw new CryptographicException("Invalid JOSE ECDSA length");
        }
    }
}

sealed class IntegerToDer : ISelect<ReadOnlyMemory<byte>, Lease<byte>>
{
    public static IntegerToDer Default { get; } = new();

    IntegerToDer() : this(NewLeasing<byte>.Default) {}

    readonly INewLeasing<byte> _new;

    public IntegerToDer(INewLeasing<byte> @new)
    {
        _new = @new;
    }

    public Lease<byte> Get(ReadOnlyMemory<byte> parameter)
    {
        // INTEGER 0 → 02 01 00
        if (parameter.Length == 0)
        {
            var zero = _new.Get(3);
            zero.Store[0] = 0x02; // INTEGER tag
            zero.Store[1] = 0x01; // length = 1
            zero.Store[2] = 0x00; // value = 0
            return zero.AsEnumerable();
        }

        var span = parameter.Span;

        // Trim leading zeros but leave at least one byte
        var i = 0;
        while (i < span.Length - 1 && span[i] == 0x00)
        {
            i++;
        }

        // Need a leading 0x00 if MSB set to keep it positive per DER
        var needZero       = (span[i] & 0x80) != 0;
        var significantLen = span.Length - i;
        var contentLen     = (needZero ? 1 : 0) + significantLen;

        // Short-form length OK for ECDSA (<=33). For >127, implement long-form.
        var result = _new.Get(2 + contentLen);

        // Tag + length
        result.Store[0] = 0x02;
        result.Store[1] = (byte)contentLen;

        var content = result.Store.AsSpan(2, contentLen);
        if (needZero)
        {
            content[0] = 0x00;
            span.Slice(i, significantLen).CopyTo(content[1..]);
        }
        else
        {
            span.Slice(i, significantLen).CopyTo(content);
        }

        return result.AsEnumerable();
    }
}