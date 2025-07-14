using System.Net.Http;

namespace DragonSpark.Presentation.Components.Grid;

public readonly record struct ClientGridParametersView(HttpClient Client, string Address);