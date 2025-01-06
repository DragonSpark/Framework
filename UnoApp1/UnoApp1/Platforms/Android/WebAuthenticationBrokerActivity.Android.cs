using Android.App;
using Android.Content.PM;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace UnoApp1.Droid;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter([Android.Content.Intent.ActionView],
              Categories = [Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable],
              DataScheme = "myprotocol"), MustDisposeResource(false)]
public class WebAuthenticationBrokerActivity : Uno.AuthenticationBroker.WebAuthenticationBrokerActivityBase;
