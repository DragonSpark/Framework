using Android.Runtime;
using Com.Nostra13.Universalimageloader.Core;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace UnoApp1.Droid;

[Android.App.Application(Label = "@string/ApplicationName",
                         Icon = "@mipmap/icon",
                         LargeHeap = true,
                         HardwareAccelerated = true, UsesCleartextTraffic = true,
                         Theme = "@style/AppTheme"), MustDisposeResource(false)]
public class Application : NativeApplication
{
    [MustDisposeResource(false)]
    public Application(IntPtr javaReference, JniHandleOwnership transfer)
        : base(() => new App(), javaReference, transfer)
    {
        // Create global configuration and initialize ImageLoader with this config
        var config = new ImageLoaderConfiguration.Builder(Context).Build();
        ImageLoader.Instance.Init(config);
        ImageSource.DefaultImageLoader = ImageLoader.Instance.LoadImageAsync;
    }
}
