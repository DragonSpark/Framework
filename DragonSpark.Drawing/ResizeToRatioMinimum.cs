using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace DragonSpark.Drawing;

sealed class ResizeToRatioMinimum : ICommand<ResizeToRatioInput>
{
	public static ResizeToRatioMinimum Default { get; } = new();

	ResizeToRatioMinimum() {}

	public void Execute(ResizeToRatioInput parameter)
	{
		var (subject, (width, height)) = parameter;
		var ratio = Math.Min(width, height).Real() / Math.Max(subject.Width, subject.Height);
		var size  = new Size((int)(subject.Width * ratio), (int)(subject.Height * ratio));
		subject.Mutate(x => x.Resize(size));
	}
}