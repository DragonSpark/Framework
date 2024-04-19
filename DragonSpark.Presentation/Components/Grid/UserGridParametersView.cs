using System.Net.Http;

namespace DragonSpark.Presentation.Components.Grid;

public readonly record struct UserGridParametersView(HttpClient Client, string Address);