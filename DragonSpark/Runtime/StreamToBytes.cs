﻿using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using JetBrains.Annotations;
using System;
using System.IO;

namespace DragonSpark.Runtime;

[UsedImplicitly]
public sealed class StreamToBytes : ISelect<Stream, Array<byte>>
{
	public static StreamToBytes Default { get; } = new();

	StreamToBytes() : this(CopyStream.Default.Get) {}

	readonly Func<Stream, MemoryStream> _copy;

	public StreamToBytes(Func<Stream, MemoryStream> copy) => _copy = copy;

	public Array<byte> Get(Stream parameter)
	{
		var stream = parameter as MemoryStream ?? _copy(parameter);
		var result = stream.ToArray();
		return result;
	}
}