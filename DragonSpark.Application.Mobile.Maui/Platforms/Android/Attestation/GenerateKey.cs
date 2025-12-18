using Android.Content;
using Android.Provider;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Attestation;

sealed class GenerateKey : IResult<string>
{
    public static GenerateKey Default { get; } = new();

    GenerateKey() : this(Platform.AppContext.ContentResolver.Verify(), Settings.Secure.AndroidId) {}

    readonly ContentResolver _resolver;
    readonly string          _key;

    public GenerateKey(ContentResolver resolver, string key)
    {
        _resolver = resolver;
        _key      = key;
    }

    public string Get() => Settings.Secure.GetString(_resolver, _key).Verify();
}