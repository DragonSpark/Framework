using DragonSpark.Model.Sequences;
using Humanizer;
using System;
using System.Linq;

namespace DragonSpark.Presentation.Model;

public sealed class EnumerationOptions<T> : Instances<Option<T>> where T : struct, Enum
{
	public static EnumerationOptions<T> Default { get; } = new();

	EnumerationOptions()
		: base(Enum.GetValues<T>().Select(x => new Option<T> { Name = x.Humanize(LetterCasing.Title), Value = x })) {}
}