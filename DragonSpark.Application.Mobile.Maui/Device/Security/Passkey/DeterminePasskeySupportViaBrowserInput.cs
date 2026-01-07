using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

public readonly record struct DeterminePasskeySupportViaBrowserInput(
    WebView Subject,
    TaskCompletionSource<bool> Source);