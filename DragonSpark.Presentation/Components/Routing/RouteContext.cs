namespace DragonSpark.Presentation.Components.Routing;

/// <summary>
/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/tree/master/CEC.Routing/Routing
/// </summary>
readonly record struct RouteContext(string Path, string[] Segments)
{
	public RouteContext(string path) : this(path, Routing.Segments.Default.Get(path)) {}
}