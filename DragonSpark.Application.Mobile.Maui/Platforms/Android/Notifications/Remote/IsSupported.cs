using Android.Content;
using Android.Gms.Common;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class IsSupported : ICondition
{
    public static IsSupported Default { get; } = new();

    IsSupported() : this(GoogleApiAvailability.Instance, Platform.AppContext) {}

    readonly GoogleApiAvailability _availability;
    readonly Context               _context;

    public IsSupported(GoogleApiAvailability availability, Context context)
    {
        _availability = availability;
        _context      = context;
    }

    public bool Get(None parameter)
        => _availability.IsGooglePlayServicesAvailable(_context) == ConnectionResult.Success;
}