using Uno.Extensions.Navigation;

namespace DragonSpark.Application.Mobile.Run.Start;

public readonly record struct RoutesInput(IViewRegistry Views, IRouteRegistry Routes);