using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Presentation.Model;

public static class Options
{
	public static Array<Option<T>> For<T>() where T : struct, Enum => EnumerationOptions<T>.Default.Get();
}