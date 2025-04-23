using System;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Uno.Extensions.Navigation;

namespace DragonSpark.Application.Mobile.Uno.Run.Start;

public class RegisterRoutes : ICommand<RoutesInput>
{
	readonly Func<IViewRegistry, RouteMap> _map;
	readonly Array<ViewMap>                _views;

	protected RegisterRoutes(Func<IViewRegistry, RouteMap> map, params ViewMap[] views)
	{
		_map   = map;
		_views = views;
	}

	public void Execute(RoutesInput parameter)
	{
		var (views, routes) = parameter;
		
        views.Register(_views);
		routes.Register(_map(views));
	}

	public void Execute(IViewRegistry views, IRouteRegistry routes)
	{
		Execute(new(views, routes));
	}
}
