using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace DragonSpark.Drawing;

sealed class CloneToRatioMaximum : ISelect<ResizeToRatioInput, Image>
{
	public static CloneToRatioMaximum Default { get; } = new();

	CloneToRatioMaximum() {}

	public Image Get(ResizeToRatioInput parameter)
	{
		var (subject, (width, height)) = parameter;
		var ratio = Math.Max(width, height).Real() / Math.Max(subject.Width, subject.Height);
		var size  = new Size((int)(subject.Width * ratio), (int)(subject.Height * ratio));
		return subject.Clone(x => x.Resize(size));
	}
}