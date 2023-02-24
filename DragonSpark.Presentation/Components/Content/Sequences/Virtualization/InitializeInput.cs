using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

public readonly record struct InitializeInput
	(ElementReference Element, IDisposable Reference, byte Direction) : IArray<object>
{
	public Array<object> Get() => new object[] { Element, Reference, Direction };
}