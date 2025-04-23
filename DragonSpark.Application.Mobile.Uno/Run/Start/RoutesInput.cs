using Uno.Extensions.Navigation;

namespace DragonSpark.Application.Mobile.Uno.Run.Start;

public readonly record struct RoutesInput(IViewRegistry Views, IRouteRegistry Routes);