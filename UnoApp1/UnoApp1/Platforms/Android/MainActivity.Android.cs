using Android.App;
using Android.Views;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace UnoApp1.Droid;

[Activity(MainLauncher = true, ConfigurationChanges = ActivityHelper.AllConfigChanges,
          WindowSoftInputMode = SoftInput.AdjustNothing | SoftInput.StateHidden), MustDisposeResource(false)]
public sealed class MainActivity : ApplicationActivity;
