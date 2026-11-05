using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.Content.History;

sealed class References : StandardTable<string, StandardTable<Type, List<object>>>
{
	public References() : base(_ => new(_ => [])) {}
}