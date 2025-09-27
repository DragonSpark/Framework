using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace DragonSpark.Drawing;

sealed class CloneToRatioMinimum : ISelect<ResizeToRatioInput, Image>
{
	public static CloneToRatioMinimum Default { get; } = new();

	CloneToRatioMinimum() {}

	public Image Get(ResizeToRatioInput parameter)
	{
		var (subject, (width, height)) = parameter;
		var ratio = Math.Min(width, height).Real() / Math.Max(subject.Width, subject.Height);
		var size  = new Size((int)(subject.Width * ratio), (int)(subject.Height * ratio));
		return subject.Clone(x => x.Resize(size));
	}
}