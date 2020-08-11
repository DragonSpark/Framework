namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/tree/master/CEC.Routing/Routing
	/// </summary>
	readonly struct RouteContext
	{
		public RouteContext(string path) : this(path, Routing.Segments.Default.Get(path)) {}

		public RouteContext(string path, string[] segments)
		{
			Path     = path;
			Segments = segments;
		}

		public string Path { get; }
		public string[] Segments { get; }
	}
}