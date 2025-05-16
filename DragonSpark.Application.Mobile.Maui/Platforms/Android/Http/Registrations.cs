using System.Net.Http;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Http;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.AddHttpClient()
                 .ConfigureHttpClientDefaults(x => x
                                                  .ConfigurePrimaryHttpMessageHandler<
                                                      Xamarin.Android.Net.AndroidMessageHandler>());
        parameter.AddTransient<HttpMessageHandler, Xamarin.Android.Net.AndroidMessageHandler>();
    }
}