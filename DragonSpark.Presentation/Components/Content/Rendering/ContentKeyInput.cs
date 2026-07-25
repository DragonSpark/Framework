namespace DragonSpark.Presentation.Components.Content.Rendering;

public readonly record struct ContentKeyInput(Type Type, int Pointer)
{
	public ContentKeyInput(object input) : this(input.GetType(), input.GetHashCode()) {}
}